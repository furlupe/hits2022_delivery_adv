using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedRestaurantsDto>> GetAllRestaurants([FromQuery] string? name, [FromQuery, BindRequired] int page = 1)
        {
            return Ok(await _restaurantService.GetAllRestaurants(page, name));
        }

        [HttpGet("{restaurantId}/menus")]
        public async Task<ActionResult<PagedMenusDto>> GetRestaurantMenus(Guid restaurantId, [FromQuery] string? name, [FromQuery, BindRequired] int page = 1)
        {
            return Ok(await _restaurantService.GetRestaurantMenus(restaurantId, page, name));
        }

        [HttpGet("{restaurantId}")]
        public async Task<ActionResult<PagedDishesDto>> GetRestaurantDishes(
            Guid restaurantId,
            [FromQuery] ICollection<FoodCategory> categories,
            [FromQuery] bool? isVegetarian,
            [FromQuery] SortingType? sortBy,
            [FromQuery] string? menu,
            int page = 1)
        {
            return Ok(await _restaurantService.GetRestaurantDishes(restaurantId, page, new Filters
            {
                Categories = categories,
                Menu = menu,
                IsVegetarian = isVegetarian,
                SortBy = sortBy
            }));
        }

        [HttpGet("menus/{menuId}")]
        public async Task<ActionResult<PagedDishesDto>> GetMenuDishes(
            Guid menuId,
            [FromQuery] ICollection<FoodCategory> categories,
            [FromQuery] bool? isVegetarian,
            [FromQuery] SortingType? sortBy,
            int page = 1)
        {
            return Ok(await _restaurantService.GetMenuDishes(menuId, page, new Filters
            {
                Categories = categories,
                IsVegetarian = isVegetarian,
                SortBy = sortBy
            }));
        }
    }
}
