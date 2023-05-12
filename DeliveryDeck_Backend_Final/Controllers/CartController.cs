using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/cart")]
    [Authorize]
    [RoleRequirementAuthorization(RoleType.Customer)]
    [ApiController]
    public class CartController : AuthorizeController
    {
        private readonly ICartService _cartService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;
        private readonly IRabbitMqService _rabbitService;
        public CartController(ICartService cartService, IResourceAuthorizationService resourceAuthorizationService, IRabbitMqService rabbitService)
        {
            _cartService = cartService;
            _resourceAuthorizationService = resourceAuthorizationService;
            _rabbitService = rabbitService;
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            _rabbitService.SendMessage(UserId.ToString(), "Gotcha cart, m8");
            return Ok(await _cartService.GetCart(UserId));
        }

        [HttpPost("{dishId}")]
        public async Task<IActionResult> AddDish(Guid dishId, [FromQuery, BindRequired] int amount = 1)
        {
            if (!await _resourceAuthorizationService.DishIsActive(dishId))
            {
                return NotFound();
            }

            await _cartService.AddDish(UserId, dishId, amount);
            return NoContent();
        }

        [HttpPatch("{dishId}/remove")]
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
