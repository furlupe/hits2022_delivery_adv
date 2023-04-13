using DeliveryDeck_Backend_Final.ClaimAuthorize;
using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
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
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ClaimPermissionRequirement(OrderPermissions.Add)]
        public async Task<ActionResult<List<Guid>>> CreateOrders(CreateOrderDto data)
        {
            return Ok(await _orderService.CreateOrder(UserId, data));
        }

        [HttpPatch("{orderId}/cancel")]
        [ClaimPermissionRequirement(OrderPermissions.Cancel)]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            await _orderService.CancelOrder(UserId, orderId);
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
            return Ok(await _orderService.GetHistory(UserId, orderNumber, fromDate, page, activeOnly));
        }

        [HttpGet("{orderNumber}")]
        [ClaimPermissionRequirement(OrderPermissions.ReadOwnOrderHistory)]
        public async Task<ActionResult<OrderDto>> GetOrderDetails(int orderNumber)
        {
            return Ok(await _orderService.GetOrderDetails(UserId, orderNumber));
        }

    }
}
