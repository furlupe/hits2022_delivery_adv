using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace AdminPanel.BLL.Services
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly UserManager<AppUser> _userMgr;
        private readonly SignInManager<AppUser> _signInMgr;
        public AdminAuthService(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr)
        {
            _userMgr = userMgr;
            _signInMgr = signInMgr;
        }
        public async Task Login(LoginDto credentials)
        {
            var user = await _userMgr.FindByEmailAsync(credentials.Email)
                ?? throw new BadHttpRequestException("No such email");

            if (!await _userMgr.IsInRoleAsync(user, RoleType.Admin.ToString()))
            {
                throw new BadHttpRequestException("Access denied", StatusCodes.Status403Forbidden);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };

            claims.AddRange((await _userMgr.GetRolesAsync(user)).Select(x => new Claim(ClaimTypes.Role, x)));

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(2),
                IsPersistent = true
            };

            await _signInMgr.SignInWithClaimsAsync(user, authProperties, claims);
        }

        public async Task Logout()
        {
            await _signInMgr.SignOutAsync();
        }
    }
}
