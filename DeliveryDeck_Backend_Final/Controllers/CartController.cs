using DeliveryDeck_Backend_Final.ClaimAuthorize;
using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/backend/cart")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [ClaimPermissionRequirement(CartPermissions.Read)]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            if (!ClaimsHelper.HasPermission(User.Claims, CartPermissions.Read))
            {
                return Forbid();
            }
            return Ok(await _cartService.GetCart(ClaimsHelper.GetUserId(User.Claims)));
        }

        [HttpPost("{dishId}")]
        [ClaimPermissionRequirement(CartPermissions.Adjust)]
        public async Task<IActionResult> AddDish(Guid dishId, [FromQuery, BindRequired] int amount = 1)
        {
            if (!ClaimsHelper.HasPermission(User.Claims, CartPermissions.Adjust))
            {
                return Forbid();
            }
            await _cartService.AddDish(ClaimsHelper.GetUserId(User.Claims), dishId, amount);
            return NoContent();
        }

        [HttpPatch("{dishId}/remove")]
        [ClaimPermissionRequirement(CartPermissions.Adjust)]
        public async Task<IActionResult> RemoveDish(Guid dishId, [FromQuery, BindRequired] int amount = 1)
        {
            if (!ClaimsHelper.HasPermission(User.Claims, CartPermissions.Adjust))
            {
                return Forbid();
            }

            await _cartService.RemoveDish(ClaimsHelper.GetUserId(User.Claims), dishId, amount);
            return NoContent();
        }

        [HttpDelete("{dishId}")]
        [ClaimPermissionRequirement(CartPermissions.Adjust)]
        public async Task<IActionResult> RemoveDishCompletely(Guid dishId)
        {
            if (!ClaimsHelper.HasPermission(User.Claims, CartPermissions.Adjust))
            {
                return Forbid();
            }

            await _cartService.RemoveDishCompletely(ClaimsHelper.GetUserId(User.Claims), dishId);
            return NoContent();
        }
    }
}
