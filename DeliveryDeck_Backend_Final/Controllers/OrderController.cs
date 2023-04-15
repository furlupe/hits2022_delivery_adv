using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : AuthorizeController
    {
        private readonly IOrderService _orderService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;
        public OrderController(IOrderService orderService, IResourceAuthorizationService resourceAuthorizationService)
        {
            _orderService = orderService;
            _resourceAuthorizationService = resourceAuthorizationService;
        }

        [HttpPost]
        [ClaimPermissionRequirement(OrderPermissions.Add)]
        public async Task<ActionResult<RemovedDishesDto>> CreateOrders(CreateOrderDto data)
        {
            return Ok(await _orderService.CreateOrder(UserId, data));
        }

        [HttpPatch("{orderNumber}/cancel")]
        [ClaimPermissionRequirement(OrderPermissions.Cancel)]
        public async Task<IActionResult> CancelOrder(int orderNumber)
        {
            if (! await _resourceAuthorizationService.OrderResourceExists(UserId, orderNumber))
            {
                return NotFound();
            }
            await _orderService.CancelOrder(orderNumber);
            return NoContent();
        }

        [HttpGet]
        [ClaimPermissionRequirement(OrderPermissions.ReadOwnOrderHistory)]
        public async Task<ActionResult<OrderPagedDto>> GetListOfActiveOrders(
            [FromQuery] bool activeOnly,
            [FromQuery, BindRequired] int page = 1,
            [FromQuery] int? orderNumber = default,
            [FromQuery] DateTime fromDate = default)
        {
            return Ok(await _orderService.GetCustomerHistory(UserId, orderNumber, fromDate, page, activeOnly));
        }

        [HttpGet("{orderNumber}")]
        [ClaimPermissionRequirement(OrderPermissions.ReadOwnOrderHistory)]
        public async Task<ActionResult<OrderDto>> GetOrderDetails(int orderNumber)
        {
            if (!await _resourceAuthorizationService.OrderResourceExists(UserId, orderNumber))
            {
                return NotFound();
            }

            return Ok(await _orderService.GetOrderDetails(orderNumber));
        }

        [HttpPost("{orderNumber}/repeat")]
        [ClaimPermissionRequirement(OrderPermissions.Add)]
        public async Task<IActionResult> RepeatOrder(int orderNumber)
        {
            if (!await _resourceAuthorizationService.OrderResourceExists(UserId, orderNumber))
            {
                return NotFound();
            }

            await _orderService.RepeatPreviousOrder(orderNumber);
            return NoContent();
        }

    }
}
