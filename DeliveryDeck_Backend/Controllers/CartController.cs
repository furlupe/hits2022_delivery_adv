using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend.Controllers
{
    [ApiController]
    [Route("cart")]
    public class CartController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetCart() 
        {
            throw new NotImplementedException();
        }

        [HttpPost("{dishId}")]
        public Task<IActionResult> AddDish([BindRequired] Guid dishId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{dishId}")]
        public Task<IActionResult> RemoveDish([BindRequired] Guid dishId, bool removeAll)
        {
            throw new NotImplementedException ();
        }
    }
}
