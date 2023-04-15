using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/cook")]
    [ApiController]
    [Authorize]
    public class CookController : AuthorizeController
    {
        private readonly IOrderService _orderService;
        private readonly IResourceAuthorizationService _resAuthorizationService;
        public CookController(IOrderService orderService, IResourceAuthorizationService resAuthorizationService)
        {
            _orderService = orderService;
            _resAuthorizationService = resAuthorizationService;
        }

        /*[HttpGet("restaurant/orders/available")]
        [ClaimPermissionRequirement(OrderPermissions.GetAvailableForCooking)]
        public async Task<ActionResult<OrderKitchenPagedDto>> GetAvailableOrders(
            [FromQuery] OrderSortingType sortBy, 
            [FromQuery, BindRequired] int page = 1
            )
        {
            return Ok(await _orderService.GetAvailableForKitchen(UserId, sortBy, page));
        }*/

        /*[HttpPatch("restaurant/orders/{orderId}/take-to-kitchen")]
        [ClaimPermissionRequirement(OrderPermissions.ChangeStatusUntilDelivery)]
        public async Task<IActionResult> TakeOrderToKitchen(int orderId)
        {
            if (! await _resAuthorizationService.RestaurantOrderExists(UserId, orderId))
            {
                return NotFound();
            }

            await _orderService.TakeOrderToKitchen(UserId, orderId);
            return NoContent();
        }

        [HttpPatch("restaurant/orders/{orderId}/package")]
        [ClaimPermissionRequirement(OrderPermissions.ChangeStatusUntilDelivery)]
        public async Task<IActionResult> PackageOrder(int orderId)
        {
            if (! await _resAuthorizationService.RestaurantOrderExists(UserId, orderId))
            {
                return NotFound();
            }

            if (! await _resAuthorizationService.OrderCookRelationExists(UserId, orderId))
            {
                return Forbid();
            }

            await _orderService.TakeOrderToPackaging(orderId);
            return NoContent();
        }

        [HttpPatch("restaurant/orders/{orderId}/set-ready-for-delivery")]
        [ClaimPermissionRequirement(OrderPermissions.ChangeStatusUntilDelivery)]
        public async Task<IActionResult> SetOrderReadyForDelivery(int orderId)
        {
            if (!await _resAuthorizationService.RestaurantOrderExists(UserId, orderId))
            {
                return NotFound();
            }

            if (!await _resAuthorizationService.OrderCookRelationExists(UserId, orderId))
            {
                return Forbid();
            }

            await _orderService.SetOrderToDeliveryAvailable(orderId);
            return NoContent();

        }*/

        [HttpPatch("restaurant/orders/{orderId}/{act}")]
        [ClaimPermissionRequirement(OrderPermissions.ChangeStatusUntilDelivery)]        
        public async Task<IActionResult> PerformActionOnOrder(int orderId, string act)
        {

            if (!await _resAuthorizationService.RestaurantOrderExists(UserId, orderId))
            {
                return NotFound();
            }

            switch(act)
            {
                case CookOrderAction.SEND_TO_KITCHEN: await _orderService.TakeOrderToKitchen(UserId, orderId); break;
                case CookOrderAction.SEND_TO_PACKAGE:
                case CookOrderAction.SET_DELIVERY_AVAILABLE:
                    if (!await _resAuthorizationService.OrderCookRelationExists(UserId, orderId))
                    {
                        return Forbid();
                    }

                    if (act == CookOrderAction.SEND_TO_PACKAGE)
                    {
                        await _orderService.TakeOrderToPackaging(orderId);
                    }
                    else if (act == CookOrderAction.SET_DELIVERY_AVAILABLE)
                    {
                        await _orderService.SetOrderToDeliveryAvailable(orderId);
                    }

                    break;
                default: return NotFound();

            }

            return NoContent();
        }
    }
}
