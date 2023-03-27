using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend.Controllers
{
    [ApiController]
    [Route("restaurants")]
    public class RestaurantController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetAllRestaurant(string startsWith)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{restaurantId}/menu")]
        public Task<IActionResult> GetRestaurantMenu([BindRequired] Guid restaurantId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("my/orders")]
        public Task<IActionResult> GetRestaurantOrders([FromQuery] List<string> filters)
        {
            throw new NotImplementedException();
        }

        [HttpGet("my/kitchen/available_orders")]
        public Task<IActionResult> GetRestaurantKitchenOrders([FromQuery] List<string> filters)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("my/menu/{menuId}/set_active")]
        public Task<IActionResult> SetActiveMenuForRestaurant(Guid menuId)
        {
            throw new NotImplementedException();
        }
    }
}
