using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using DeliveryDeck_Backend_Final.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryDeck_Backend_Final.Auth.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            return Ok(await _userService.GetProfile(ClaimsHelper.GetUserId(User.Claims)));
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UserUpdateProfileDto data)
        {
            await _userService.UpdateProfile(ClaimsHelper.GetUserId(User.Claims), data);
            return Ok();
        }

        [HttpPatch("change_password")]
        [Authorize]

        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwords)
        {
            await _userService.ChangePassword(ClaimsHelper.GetUserId(User.Claims), passwords);
            return Ok();
        }
    }
}
