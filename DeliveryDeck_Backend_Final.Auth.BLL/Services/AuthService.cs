﻿using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Auth.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userMgr;
        private readonly RoleManager<Role> _roleMgr;
        private readonly AuthContext _authContext;
        private readonly ITokenService _tokenService;
        public AuthService(UserManager<AppUser> userManager, RoleManager<Role> roleManager, AuthContext authContext, ITokenService tokenService)
        {
            _userMgr = userManager;
            _authContext = authContext;
            _tokenService = tokenService;
            _roleMgr = roleManager;
        }

        public async Task ChangePassword(Guid userId, ChangePasswordDto passwords)
        {
            var user = await _userMgr.FindByIdAsync(userId.ToString());
            var result = await _userMgr.ChangePasswordAsync(user, passwords.OldPassword, passwords.NewPassword);

            if (!result.Succeeded)
            {
                throw new BadHttpRequestException("Could not change the password");
            }

        }

        public async Task<TokenPairDto> Login(LoginCredentials credentials)
        {
            var user = await _userMgr.FindByEmailAsync(credentials.Email);
            if (user == null || !await _userMgr.CheckPasswordAsync(user, credentials.Password))
            {
                throw new BadHttpRequestException("Invalid credentials");
            }

            return await CreateTokenPair(user);

        }

        public async Task Logout(Guid userId)
        {
            await _authContext.RefreshTokens
                .Where(t => t.User.Id == userId)
                .ExecuteDeleteAsync();

            await _authContext.SaveChangesAsync();
        }

        public async Task<TokenPairDto> Refresh(string refreshToken)
        {
            var rt = await _authContext.RefreshTokens
                .Include(x => x.User)
                .SingleOrDefaultAsync(rt => rt.Value == refreshToken)
                ?? throw new BadHttpRequestException("Invalid refresh token");

            return await CreateTokenPair(rt.User);
        }

        public async Task Register(UserRegistrationDto data)
        {
            AppUser user = new()
            {
                FullName = data.FullName,
                BirthDate = data.BirthDate,
                Gender = data.Gender,
                PhoneNumber = data.Phone,
                Email = data.Email,
            };

            var result = await _userMgr.CreateAsync(user, data.Password);

            if (!result.Succeeded)
            {
                throw new BadImageFormatException("Could not register");
            }

            await _userMgr.AddToRoleAsync(user, RoleType.Customer.ToString());
        }

        private async Task<TokenPairDto> CreateTokenPair(AppUser user)
        {
            var rt = _tokenService.CreateRefreshToken();
            var existingUserRefresh = await _authContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.User == user);

            if (existingUserRefresh != null)
            {
                await _authContext.RefreshTokens.Where(rt => rt == existingUserRefresh).ExecuteDeleteAsync();
            }

            await _authContext.AddAsync(new RefreshUserToken
            {
                Value = rt.Value,
                User = user
            });

            await _authContext.SaveChangesAsync();

            return new TokenPairDto
            {
                AccessToken = _tokenService.CreateAccessToken(await CreateIdentity(user)),
                RefreshToken = rt
            };
        }

        private async Task<ClaimsIdentity> CreateIdentity(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };

            foreach (var role in await _userMgr.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                var roleClaims = await _roleMgr
                    .GetClaimsAsync
                        (user.Roles.First(r => r.Role.Name == role).Role
                    );

                foreach (var claim in roleClaims)
                {
                    claims.Add(claim);
                }
            }

            return new ClaimsIdentity(claims, "Token");

        }

    }
}
