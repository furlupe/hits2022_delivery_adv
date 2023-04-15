using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/backend/cart")]
    [Authorize]
    [ApiController]
    public class CartController : AuthorizeController
    {
        private readonly ICartService _cartService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;
        public CartController(ICartService cartService, IResourceAuthorizationService resourceAuthorizationService)
        {
            _cartService = cartService;
            _resourceAuthorizationService = resourceAuthorizationService;
        }

        [HttpGet]
        [ClaimPermissionRequirement(CartPermissions.Read)]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            return Ok(await _cartService.GetCart(UserId));
        }

        [HttpPost("{dishId}")]
        [ClaimPermissionRequirement(CartPermissions.Adjust)]
        public async Task<IActionResult> AddDish(Guid dishId, [FromQuery, BindRequired] int amount = 1)
        {
            if (! await _resourceAuthorizationService.DishResourceExists(dishId))
            {
                return NotFound();
            }

            await _cartService.AddDish(UserId, dishId, amount);
            return NoContent();
        }

        [HttpPatch("{dishId}/remove")]
        [ClaimPermissionRequirement(CartPermissions.Adjust)]
        public async Task<IActionResult> RemoveDish(Guid dishId, [FromQuery, BindRequired] int amount = 1)
        {
            if (
                !(await _resourceAuthorizationService.DishResourceExists(dishId)
                && await _resourceAuthorizationService.DishInCartResourceExists(UserId, dishId))
                )
            {
                return NotFound();
            }

            await _cartService.RemoveDish(UserId, dishId, amount);
            return NoContent();
        }

        [HttpDelete("{dishId}")]
        [ClaimPermissionRequirement(CartPermissions.Adjust)]
        public async Task<IActionResult> RemoveDishCompletely(Guid dishId)
        {
            if (
                !(await _resourceAuthorizationService.DishResourceExists(dishId)
                && await _resourceAuthorizationService.DishInCartResourceExists(UserId, dishId))
                )
            {
                return NotFound();
            }

            await _cartService.RemoveDishCompletely(UserId, dishId);
            return NoContent();
        }
    }
}
