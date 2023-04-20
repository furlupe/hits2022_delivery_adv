﻿using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/cook")]
    [ApiController]
    [RoleRequirementAuthorization(RoleType.Cook)]
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

        [HttpGet("restaurant/orders/available")]
        public async Task<ActionResult<OrderAvailablePagedDto>> GetAvailableOrders(
            [FromQuery] OrderSortingItem? sort,
            [FromQuery] OrderSortingItem? desc,
            [FromQuery, BindRequired] int page = 1
            )
        {
            var sortBy = (desc is not null) ? desc.ToSortingType(true) : sort.ToSortingType();
            return Ok(await _orderService.GetAvailableForKitchen(UserId, sortBy, page));
        }

        [HttpGet("orders")]
        public async Task<ActionResult<OrderPagedDto>> GetOrderHistory(
           [FromQuery] int? number,
           [FromQuery] DateTime fromDate,
           [FromQuery, BindRequired] int page = 1
            )
        {
            return await _orderService.GetCookHistory(UserId, number, fromDate, page);
        }

        [HttpGet("orders/{orderNumber}")]
        public async Task<ActionResult<OrderDto>> GetOrderDetails(int orderNumber)
        {
            if (!await _resAuthorizationService.OrderCookRelationExists(UserId, orderNumber))
            {
                return NotFound();
            }

            return await _orderService.GetOrderDetails(orderNumber);
        }

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
        public async Task<IActionResult> PerformActionOnOrder(int orderId, OrderAction act)
        {

            if (!await _resAuthorizationService.StaffRestaurantOrderResourceExists(UserId, orderId))
            {
                return NotFound();
            }

            switch (act)
            {
                case OrderAction.kitchen: await _orderService.TakeOrderToKitchen(UserId, orderId); break;
                case OrderAction.package:
                case OrderAction.deliverable:
                    if (!await _resAuthorizationService.OrderCookRelationExists(UserId, orderId))
                    {
                        return Forbid();
                    }

                    if (act == OrderAction.package)
                    {
                        await _orderService.TakeOrderToPackaging(orderId);
                    }
                    else if (act == OrderAction.deliverable)
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
