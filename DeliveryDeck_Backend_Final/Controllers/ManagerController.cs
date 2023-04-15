using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.CustomPermissions;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/manager/restaurant")]
    [ApiController]
    [Authorize]
    public class ManagerController : AuthorizeController
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;

        public ManagerController(IRestaurantService restaurantService, IResourceAuthorizationService resourceAuthorizationService)
        {
            _restaurantService = restaurantService;
            _resourceAuthorizationService = resourceAuthorizationService;
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
            if (!await _resourceAuthorizationService.RestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            return Ok(await _restaurantService.GetMenuDetails(UserId, menuId, page));
        }

        [HttpPatch("menus/{menuId}")]
        [ClaimPermissionRequirement(MenuPermissions.Adjust)]
        public async Task<IActionResult> UpdateMenu(Guid menuId, UpdateMenuDto data)
        {
            if (!await _resourceAuthorizationService.RestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.UpdateMenu(UserId, menuId, data);
            return NoContent();
        }

        [HttpDelete("menus/{menuId}")]
        [ClaimPermissionRequirement(MenuPermissions.Adjust)]
        public async Task<IActionResult> DeleteMenu(Guid menuId)
        {
            if (!await _resourceAuthorizationService.RestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.DeleteMenu(UserId, menuId);
            return NoContent();
        }

        [HttpPost("menus/{menuId}/dishes")]
        [ClaimPermissionRequirement(MenuPermissions.Adjust)]
        public async Task<IActionResult> AddDishesToMenu(Guid menuId, SomeDishesForMenuDto data)
        {
            if (!await _resourceAuthorizationService.RestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.AddDishesToMenu(UserId, menuId, data.Dishes);
            return NoContent();
        }

        [HttpDelete("menus/{menuId}/dishes")]
        [ClaimPermissionRequirement(MenuPermissions.Adjust)]
        public async Task<IActionResult> RemoveDishesFromMenu(Guid menuId, SomeDishesForMenuDto data)
        {
            if (!await _resourceAuthorizationService.RestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

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
            if (!await _resourceAuthorizationService.RestaurantDishResourceExists(UserId, dishId))
            {
                return NotFound();
            }

            await _restaurantService.UpdateDish(UserId, dishId, data);
            return NoContent();
        }

        [HttpDelete("dishes/{dishId}")]
        [ClaimPermissionRequirement(DishPermissions.CUD)]
        public async Task<IActionResult> RemoveDish(Guid dishId)
        {
            if (!await _resourceAuthorizationService.RestaurantDishResourceExists(UserId, dishId))
            {
                return NotFound();
            }

            await _restaurantService.RemoveDishFromRestaurant(UserId, dishId);
            return NoContent();
        }
    }
}
