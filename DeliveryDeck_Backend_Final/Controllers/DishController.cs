using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/dishes")]
    [ApiController]
    public class DishController : AuthorizeController
    {
        private readonly IDishService _dishService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;
        public DishController(IDishService dishService, IResourceAuthorizationService resourceAuthorizationService)
        {
            _dishService = dishService;
            _resourceAuthorizationService = resourceAuthorizationService;
        }

        [HttpGet("{dishId}")]
        public async Task<ActionResult<DishDto>> GetDish(Guid dishId)
        {
            if (!await _resourceAuthorizationService.DishIsActive(dishId))
            {
                return NotFound();
            }

            return Ok(await _dishService.GetDishById(dishId));
        }

        [HttpPost("{dishId}/rate")]
        [RoleRequirementAuthorization(RoleType.Customer)]
        [Authorize]
        public async Task<ActionResult<DishDto>> RateDish(Guid dishId, RatingDto data)
        {
            if (!await _resourceAuthorizationService.DishResourceExists(dishId))
            {
                return NotFound();
            }

            await _dishService.RateDish(UserId, dishId, data.Value);
            return NoContent();
        }
    }
}
