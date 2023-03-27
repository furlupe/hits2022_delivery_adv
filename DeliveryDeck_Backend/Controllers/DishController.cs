using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend.Controllers
{
    [ApiController]
    [Route("dishes")]
    public class DishController : ControllerBase
    {
        [HttpGet("{dishId}")]
        public Task<IActionResult> GetDishDetails([BindRequired] Guid dishId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{dishId}/rate")]
        public Task<IActionResult> RateDish([BindRequired] Guid dishId)
        {
            throw new NotImplementedException();
        }
    }
}
