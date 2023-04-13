using DeliveryDeck_Backend_Final.ClaimAuthorize;
using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/restaurants/my")]
    [ApiController]
    [Authorize]
    public class RestaurantManagerController : AuthorizeController
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantManagerController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("menus")]
        [ClaimPermissionRequirement(MenuPermissions.Read)]
        public async Task<ActionResult<PagedMenusDto>> GetMenus([FromQuery, BindRequired] int page = 1)
        {
            return Ok(await _restaurantService.GetMenus(UserId, page));
        }

        [HttpPost("menus")]
        [ClaimPermissionRequirement(MenuPermissions.Create)]
        public async Task<IActionResult> CreateMenu(CreateMenuDto data)
        {
            await _restaurantService.CreateMenu(UserId, data);
            return NoContent();
        }

        [HttpGet("menus/{menuId}")]
        [ClaimPermissionRequirement(MenuPermissions.Read)]
        public async Task<ActionResult<MenuInfo>> GetMenu(Guid menuId, [FromQuery, BindRequired] int page = 1)
        {
            return Ok(await _restaurantService.GetMenuDetails(UserId, menuId, page));
        }

        [HttpPatch("menus/{menuId}")]
        [ClaimPermissionRequirement(MenuPermissions.Adjust)]
        public async Task<IActionResult> UpdateMenu(Guid menuId, UpdateMenuDto data)
        {
            await _restaurantService.UpdateMenu(UserId, menuId, data);
            return NoContent();
        }

        [HttpPost("menus/{menuId}/dishes")]
        [ClaimPermissionRequirement(MenuPermissions.Adjust)]
        public async Task<IActionResult> AddDishesToMenu(Guid menuId, SomeDishesForMenuDto data)
        {
            await _restaurantService.AddDishesToMenu(UserId, menuId, data.Dishes);
            return NoContent();
        }

        [HttpDelete("menus/{menuId}/dishes")]
        [ClaimPermissionRequirement(MenuPermissions.Adjust)]
        public async Task<IActionResult> RemoveDishesFromMenu(Guid menuId, SomeDishesForMenuDto data)
        {
            await _restaurantService.RemoveDishesFromMenu(UserId, menuId, data.Dishes);
            return NoContent();
        }

        [HttpGet("dishes")]
        [ClaimPermissionRequirement(DishPermissions.Read)]
        public async Task<ActionResult<PagedDishesDto>> GetDishes([FromQuery, BindRequired] int page = 1)
        {
            return Ok(await _restaurantService.GetAllRestaurantDishes(UserId, page));
        }

        [HttpPost("dishes")]
        [ClaimPermissionRequirement(DishPermissions.CUD)]
        public async Task<IActionResult> AddDish(CreateDishDto data)
        {
            await _restaurantService.AddDishToRestaurant(UserId, data);
            return NoContent();
        }

        [HttpPatch("dishes/{dishId}")]
        [ClaimPermissionRequirement(DishPermissions.CUD)]
        public async Task<IActionResult> UpdateDish(Guid dishId, CreateDishDto data)
        {
            await _restaurantService.UpdateDish(UserId, dishId, data);
            return NoContent();
        }

        [HttpDelete("dishes/{dishId}")]
        [ClaimPermissionRequirement(DishPermissions.CUD)]
        public async Task<IActionResult> RemoveDish(Guid dishId)
        {
            await _restaurantService.RemoveDishFromRestaurant(UserId, dishId);
            return NoContent();
        }
    }
}
