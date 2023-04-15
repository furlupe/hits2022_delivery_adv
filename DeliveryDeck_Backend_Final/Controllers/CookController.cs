using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/cook")]
    [ApiController]
    [Authorize]
    public class CookController : AuthorizeController
    {
        private readonly IOrderService _orderService;
        public CookController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("restaurant/available_orders")]
        [ClaimPermissionRequirement(OrderPermissions.GetAvailableForCooking)]
        public async Task<ActionResult<OrderKitchenPagedDto>> GetAvailableOrders(
            [FromQuery] OrderSortingType sortBy, 
            [FromQuery, BindRequired] int page = 1
            )
        {
            return Ok(await _orderService.GetAvailableForKitchen(UserId, sortBy, page));
        }
    }
}
