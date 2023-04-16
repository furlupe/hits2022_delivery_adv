using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Common.Utils;
using DeliveryDeck_Backend_Final.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/courier")]
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
        [ClaimPermissionRequirement(OrderPermissions.ReadOwnDeliveryHistory)]
        public async Task<ActionResult<OrderPagedDto>> GetOrderHistory(
           [FromQuery] int? number,
           [FromQuery] DateTime fromDate,
           [FromQuery, BindRequired] int page = 1
            )
        {
            return await _orderService.GetCourierHistory(UserId, number, fromDate, page);
        }

        [HttpGet("orders/available")]
        [ClaimPermissionRequirement(OrderPermissions.GetAvailableForDelivery)]
        public async Task<ActionResult<OrderAvailablePagedDto>> GetAvailableOrders(
            [FromQuery] OrderSortingType sortBy,
            [FromQuery, BindRequired] int page = 1
            )
        {
            return Ok(await _orderService.GetAvailableForDelivery(UserId, sortBy, page));
        }

        [HttpGet("orders/{orderNumber}")]
        [ClaimPermissionRequirement(OrderPermissions.ReadOwnDeliveryHistory)]
        public async Task<ActionResult<OrderDto>> GetOrderDetails(int orderNumber)
        {
            if (!await _resourceAuthorizationService.OrderIsAccessibleForCourier(UserId, orderNumber))
            {
                return NotFound();
            }

            return await _orderService.GetOrderDetails(orderNumber);
        }

        [HttpPatch("orders/{orderNumber}/{act}")]
        [ClaimPermissionRequirement(OrderPermissions.ChangeStatusUntilDelivered)]
        public async Task<IActionResult> PerformActionOnOrder(int orderNumber, string act)
        {
            if (!await _resourceAuthorizationService.OrderIsAccessibleForCourier(UserId, orderNumber))
            {
                return NotFound();
            }

            switch (act)
            {
                case OrderAction.TAKE_TO_DELIVERY: await _orderService.SetOrderAsBeingDelivered(UserId, orderNumber); break;
                case OrderAction.SET_AS_DELIVERED:
                case OrderAction.CANCEL:
                    if (! await _resourceAuthorizationService.OrderCourierRelationExists(UserId, orderNumber))
                    {
                        return Forbid();
                    }

                    if (act == OrderAction.SET_AS_DELIVERED)
                    {
                        await _orderService.SetOrderAsDelivered(orderNumber);
                    } 
                    else if (act == OrderAction.CANCEL)
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
