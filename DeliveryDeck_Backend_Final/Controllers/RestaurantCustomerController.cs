using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantCustomerController : AuthorizeController
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;

        public RestaurantCustomerController(IRestaurantService restaurantService, IResourceAuthorizationService resourceAuthorizationService)
        {
            _restaurantService = restaurantService;
            _resourceAuthorizationService = resourceAuthorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedRestaurantsDto>> GetAllRestaurants([FromQuery] string? name, [FromQuery, BindRequired] int page = 1)
        {
            return Ok(await _restaurantService.GetAllRestaurants(page, name));
        }

        [HttpGet("{restaurantId}/menus")]
        public async Task<ActionResult<PagedMenusDto>> GetRestaurantMenus(
            Guid restaurantId,
            [FromQuery] string? name,
            [FromQuery, BindRequired] int page = 1
            )
        {
            if (!await _resourceAuthorizationService.RestaurantResourceExists(restaurantId))
            {
                return NotFound();
            }

            return Ok(await _restaurantService.GetActiveRestaurantMenus(restaurantId, page, name));
        }

        [HttpGet("{restaurantId}/dishes")]
        public async Task<ActionResult<PagedDishesDto>> GetRestaurantDishes(
            Guid restaurantId,
            [FromQuery] ICollection<FoodCategory> categories,
            [FromQuery] bool? isVegetarian,
            [FromQuery] DishSortingItem? sort,
            [FromQuery] DishSortingItem? desc,
            [FromQuery] Guid? menu,
            [FromQuery, BindRequired] int page = 1)
        {
            if (!await _resourceAuthorizationService.RestaurantResourceExists(restaurantId))
            {
                return NotFound();
            }

            DishSortingType? sortBy = (desc is not null) ? desc.ToSortingType(true) : sort.ToSortingType();

            return Ok(await _restaurantService.GetActiveRestaurantDishes(restaurantId, page, new DishFilters
            {
                Categories = categories,
                Menu = menu,
                IsVegetarian = isVegetarian,
                SortBy = sortBy
            }));
        }

        [HttpGet("menus/{menuId}/dishes")]
        public async Task<ActionResult<PagedDishesDto>> GetMenuDishes(
            Guid menuId,
            [FromQuery] ICollection<FoodCategory> categories,
            [FromQuery] bool? isVegetarian,
            [FromQuery] DishSortingItem? sort,
            [FromQuery] DishSortingItem? desc,
            [FromQuery, BindRequired] int page = 1)
        {
            if (!await _resourceAuthorizationService.MenuResourceExists(menuId))
            {
                return NotFound();
            }

            DishSortingType? sortBy = (desc is not null) ? desc.ToSortingType(true) : sort.ToSortingType();

            return Ok(await _restaurantService.GetActiveMenuDishes(menuId, page, new DishFilters
            {
                Categories = categories,
                IsVegetarian = isVegetarian,
                SortBy = sortBy
            }));
        }


    }
}
