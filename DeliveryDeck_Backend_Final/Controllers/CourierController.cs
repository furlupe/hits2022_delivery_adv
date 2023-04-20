using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/courier")]
    [RoleRequirementAuthorization(RoleType.Courier)]
    [ApiController]
    public class CourierController : AuthorizeController
    {
        private readonly IOrderService _orderService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;
        public CourierController(IOrderService orderService, IResourceAuthorizationService resourceAuthorizationService)
        {
            _orderService = orderService;
            _resourceAuthorizationService = resourceAuthorizationService;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<OrderPagedDto>> GetOrderHistory(
           [FromQuery] int? number,
           [FromQuery] DateTime fromDate,
           [FromQuery, BindRequired] int page = 1
            )
        {
            return await _orderService.GetCourierHistory(UserId, number, fromDate, page);
        }

        [HttpGet("orders/available")]
        public async Task<ActionResult<OrderAvailablePagedDto>> GetAvailableOrders(
            [FromQuery] OrderSortingItem? sort,
            [FromQuery] OrderSortingItem? desc,
            [FromQuery, BindRequired] int page = 1
            )
        {
            var sortBy = (desc is not null) ? desc.ToSortingType(true) : sort.ToSortingType();

            return Ok(await _orderService.GetAvailableForDelivery(UserId, sortBy, page));
        }

        [HttpGet("orders/{orderNumber}")]
        public async Task<ActionResult<OrderDto>> GetOrderDetails(int orderNumber)
        {
            if (!await _resourceAuthorizationService.OrderIsAccessibleForCourier(UserId, orderNumber))
            {
                return NotFound();
            }

            return await _orderService.GetOrderDetails(orderNumber);
        }

        [HttpPatch("orders/{orderNumber}/{act}")]
        public async Task<IActionResult> PerformActionOnOrder(int orderNumber, OrderAction act)
        {
            if (!await _resourceAuthorizationService.OrderIsAccessibleForCourier(UserId, orderNumber))
            {
                return NotFound();
            }

            switch (act)
            {
                case OrderAction.delivering: await _orderService.SetOrderAsBeingDelivered(UserId, orderNumber); break;
                case OrderAction.delivered:
                case OrderAction.cancel:
                    if (!await _resourceAuthorizationService.OrderCourierRelationExists(UserId, orderNumber))
                    {
                        return Forbid();
                    }

                    if (act == OrderAction.delivered)
                    {
                        await _orderService.SetOrderAsDelivered(orderNumber);
                    }
                    else if (act == OrderAction.cancel)
                    {
                        await _orderService.CancelOrder(orderNumber);
                    }

                    break;
                default: return NotFound();

            }
            return NoContent();
        }
    }
}
