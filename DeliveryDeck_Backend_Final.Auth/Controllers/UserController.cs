using DeliveryDeck_Backend_Final.Common.DTO.Auth;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Auth;
using DeliveryDeck_Backend_Final.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace DeliveryDeck_Backend_Final.Auth.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : AuthorizeController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        [RoleRequirementAuthorization(RoleType.Customer)]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            return Ok(await _userService.GetProfile(UserId));
        }

        [HttpPatch]
        [Authorize]
        [RoleRequirementAuthorization(RoleType.Customer)]
        public async Task<IActionResult> UpdateProfile(UserUpdateProfileDto data)
        {
            await _userService.UpdateProfile(UserId, data);
            return NoContent();
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto data)
        {
            await _userService.ResetPassword(data);
            return NoContent();
        }

        [HttpPost("password-forgot")]
        public async Task<ActionResult<ResetPasswordToken>> GetResetPasswordToken(ResetPasswordShortDto data)
        {
            return Ok(await _userService.GetResetPasswordToken(data.Email));
        }
    }
}
