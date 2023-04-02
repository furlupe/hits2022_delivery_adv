using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using DeliveryDeck_Backend_Final.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryDeck_Backend_Final.Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenPairDto>> Register(UserRegistrationDto registrationInfo)
        {
            await _authService.Register(registrationInfo);
            return Ok(await _authService.Login(new LoginCredentials
            {
                Email = registrationInfo.Email,
                Password = registrationInfo.Password
            }));
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenPairDto>> Login(LoginCredentials credentials)
        {
            return Ok(await _authService.Login(credentials));
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenPairDto>> Refresh(RefreshDto token)
        {
            return Ok(await _authService.Refresh(token.Value));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(ClaimsHelper.GetUserId(User.Claims));
            return Ok();
        }

    }
}
