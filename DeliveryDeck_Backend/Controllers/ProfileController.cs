using Microsoft.AspNetCore.Mvc;

namespace DeliveryDeck_Backend.Controllers
{
    [ApiController]
    [Route("profile")]
    public class ProfileController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetProfile() 
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        public Task<IActionResult> UpdateProfile() 
        { 
            throw new NotImplementedException(); 
        }
    }
}
