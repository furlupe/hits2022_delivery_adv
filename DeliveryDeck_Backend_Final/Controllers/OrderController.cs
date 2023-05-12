using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [RoleRequirementAuthorization(RoleType.Customer)]
    [Authorize]
    public class OrderController : AuthorizeController
    {
        private readonly IOrderService _orderService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;
        private readonly IRabbitMqService _rabbitService;

        public OrderController(IOrderService orderService, IResourceAuthorizationService resourceAuthorizationService, IRabbitMqService rabbitService)
        {
            _orderService = orderService;
            _resourceAuthorizationService = resourceAuthorizationService;
            _rabbitService = rabbitService;
        }

        [HttpPost]
        public async Task<ActionResult<RemovedDishesDto>> CreateOrders(CreateOrderDto data)
        {
            return Ok(await _orderService.CreateOrder(UserId, data));
        }

        [HttpPatch("{orderNumber}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderNumber)
        {
            if (!await _resourceAuthorizationService.OrderCustomerRelationExists(UserId, orderNumber))
            {
                return NotFound();
            }
            await _orderService.CancelOrder(orderNumber);
            _rabbitService.SendMessage(UserId.ToString(), $"Order [No. {orderNumber}] now has status [{OrderStatus.Cancelled}]");
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<OrderPagedDto>> GetOrderHistory(
            [FromQuery] bool activeOnly,
            [FromQuery, BindRequired] int page = 1,
            [FromQuery] int? orderNumber = default,
            [FromQuery] DateTime fromDate = default)
        {
            return Ok(await _orderService.GetCustomerHistory(UserId, orderNumber, fromDate, page, activeOnly));
        }

        [HttpGet("{orderNumber}")]
        public async Task<ActionResult<OrderDto>> GetOrderDetails(int orderNumber)
        {
            if (!await _resourceAuthorizationService.OrderCustomerRelationExists(UserId, orderNumber))
            {
                return NotFound();
            }

            return Ok(await _orderService.GetOrderDetails(orderNumber));
        }

        [HttpPost("{orderNumber}")]
        public async Task<IActionResult> RepeatOrder(int orderNumber)
        {
            if (!await _resourceAuthorizationService.OrderCustomerRelationExists(UserId, orderNumber))
            {
                return NotFound();
            }

            await _orderService.RepeatPreviousOrder(orderNumber);
            return NoContent();
        }

    }
}
