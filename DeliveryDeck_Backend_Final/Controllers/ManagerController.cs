using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/manager/restaurant")]
    [ApiController]
    [RoleRequirementAuthorization(RoleType.Manager)]
    [Authorize]
    public class ManagerController : AuthorizeController
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IDishService _dishService;
        private readonly IOrderService _orderService;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;

        public ManagerController(
            IRestaurantService restaurantService,
            IResourceAuthorizationService resourceAuthorizationService,
            IOrderService orderService,
            IDishService dishSerivce)
        {
            _restaurantService = restaurantService;
            _resourceAuthorizationService = resourceAuthorizationService;
            _orderService = orderService;
            _dishService = dishSerivce;
        }

        [HttpGet("menus")]
        public async Task<ActionResult<PagedMenusDto>> GetMenus([FromQuery, BindRequired] int page = 1)
        {
            return Ok(await _restaurantService.GetMenus(UserId, page));
        }

        [HttpPost("menus")]
        public async Task<IActionResult> CreateMenu(CreateMenuDto data)
        {
            await _restaurantService.CreateMenu(UserId, data);
            return NoContent();
        }

        [HttpGet("menus/{menuId}")]
        public async Task<ActionResult<MenuInfo>> GetMenu(Guid menuId, [FromQuery, BindRequired] int page = 1)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            return Ok(await _restaurantService.GetMenuDetails(UserId, menuId, page));
        }

        [HttpPut("menus/{menuId}/active")]
        public async Task<IActionResult> SetMenuActive(Guid menuId)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.ChangeMenuVisibility(UserId, menuId);
            return NoContent();
        }

        [HttpPut("menus/{menuId}/non-active")]
        public async Task<IActionResult> SetMenuNonActive(Guid menuId)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.ChangeMenuVisibility(UserId, menuId, false);
            return NoContent();
        }

        [HttpPut("menus/{menuId}")]
        public async Task<IActionResult> UpdateMenu(Guid menuId, UpdateMenuDto data)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.UpdateMenu(UserId, menuId, data);
            return NoContent();
        }

        [HttpDelete("menus/{menuId}")]
        public async Task<IActionResult> DeleteMenu(Guid menuId)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.DeleteMenu(UserId, menuId);
            return NoContent();
        }

        [HttpPost("menus/{menuId}/dishes")]
        public async Task<IActionResult> AddDishesToMenu(Guid menuId, SomeDishesForMenuDto data)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.AddDishesToMenu(UserId, menuId, data.Dishes);
            return NoContent();
        }

        [HttpDelete("menus/{menuId}/dishes")]
        public async Task<IActionResult> RemoveDishesFromMenu(Guid menuId, SomeDishesForMenuDto data)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantMenuResourceExists(UserId, menuId))
            {
                return NotFound();
            }

            await _restaurantService.RemoveDishesFromMenu(UserId, menuId, data.Dishes);
            return NoContent();
        }

        [HttpGet("dishes")]
        public async Task<ActionResult<PagedDishesDto>> GetDishes([FromQuery, BindRequired] int page = 1)
        {
            return Ok(await _restaurantService.GetAllRestaurantDishes(UserId, page));
        }

        [HttpGet("dishes/{dishId}")]
        public async Task<ActionResult<DishDto>> GetDishDetails(Guid dishId)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantDishResourceExists(UserId, dishId))
            {
                return NotFound();
            }
            return Ok(await _dishService.GetDishById(dishId));
        }

        [HttpPost("dishes")]
        public async Task<IActionResult> AddDish(CreateDishDto data)
        {
            await _restaurantService.AddDishToRestaurant(UserId, data);
            return NoContent();
        }

        [HttpPut("dishes/{dishId}")]
        public async Task<IActionResult> UpdateDish(Guid dishId, CreateDishDto data)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantDishResourceExists(UserId, dishId))
            {
                return NotFound();
            }

            await _restaurantService.UpdateDish(UserId, dishId, data);
            return NoContent();
        }

        [HttpDelete("dishes/{dishId}")]
        public async Task<IActionResult> RemoveDish(Guid dishId)
        {
            if (!await _resourceAuthorizationService.ManagerRestaurantDishResourceExists(UserId, dishId))
            {
                return NotFound();
            }

            await _restaurantService.RemoveDishFromRestaurant(UserId, dishId);
            return NoContent();
        }

        [HttpGet("orders")]
        public async Task<ActionResult<PagedDishesDto>> GetRestaurantOrderHistory(
            [FromQuery] OrderStatus? status,
            [FromQuery] OrderSortingItem? sort,
            [FromQuery] OrderSortingItem? desc,
            [FromQuery] int? number,
            [FromQuery, BindRequired] int page = 1
            )
        {
            OrderSortingType? sortBy = (desc is not null) ? desc.ToSortingType(true) : sort.ToSortingType();

            return Ok(await _orderService.GetRestaurantHistory(UserId, status, sortBy, number, page));
        }

        [HttpGet("orders/{orderNumber}")]
        public async Task<ActionResult<PagedDishesDto>> GetRestaurantOrderHistory(int orderNumber)
        {
            if (!await _resourceAuthorizationService.StaffRestaurantOrderResourceExists(UserId, orderNumber))
            {
                return NotFound();
            }

            return Ok(await _orderService.GetOrderDetails(orderNumber));
        }
    }
}
