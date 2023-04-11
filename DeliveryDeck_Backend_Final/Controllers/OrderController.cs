using DeliveryDeck_Backend_Final.ClaimAuthorize;
using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ClaimPermissionRequirement(OrderPermissions.Add)]
        public async Task<IActionResult> CreateOrders(CreateOrderDto data)
        {
            await _orderService.CreateOrder(ClaimsHelper.GetUserId(User.Claims), data);
            return NoContent();
        }

    }
}
