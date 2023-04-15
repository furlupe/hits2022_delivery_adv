using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/dishes")]
    [ApiController]
    public class DishController : ControllerBase
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
            if (! await _resourceAuthorizationService.DishResourceExists(dishId))
            {
                return NotFound();
            }

            return Ok(await _dishService.GetDishById(dishId));
        }
    }
}
