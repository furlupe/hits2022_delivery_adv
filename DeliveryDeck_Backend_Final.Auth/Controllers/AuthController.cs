using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Auth.Controllers
{
    [ApiController]
    [Route("auth")]
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
        public async Task<IActionResult> Refresh(RefreshDto token)
        {
            return Ok(await _authService.Refresh(token.Value));
        }

        [Authorize]
        [HttpPatch("change_password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwords)
        {
            await _authService.ChangePassword(GetUserIdFromClaims(), passwords);
            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(GetUserIdFromClaims());
            return Ok();
        }

        private Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            _ = Guid.TryParse(userIdClaim.Value, out Guid userId);

            return userId;
        }

    }
}
