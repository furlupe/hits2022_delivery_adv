using Microsoft.AspNetCore.Mvc;

namespace DeliveryDeck_Backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public Task<IActionResult> Register()
        {
            throw new NotImplementedException();
        }

        [HttpPost("login")]
        public Task<IActionResult> Login() 
        {
            throw new NotImplementedException();
        }

        [HttpPost("refresh")]
        public Task<IActionResult> Refresh() 
        {
            throw new NotImplementedException();
        }

        [HttpPatch("change_password")]
        public Task<IActionResult> ChangePassword() 
        {
            throw new NotImplementedException();
        }

        [HttpPost("logout")]
        public Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }

    }
}
