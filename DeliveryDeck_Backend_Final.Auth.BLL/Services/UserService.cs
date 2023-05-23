using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Auth;
using DeliveryDeck_Backend_Final.Common.Exceptions;
using DeliveryDeck_Backend_Final.Common.Interfaces.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Auth.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userMgr;
        private readonly AuthContext _authContext;

        public UserService(UserManager<AppUser> userMgr, AuthContext auth)
        {
            _userMgr = userMgr;
            _authContext = auth;
        }

        public async Task<UserProfileDto> GetProfile(Guid userId)
        {
            var user = await _authContext.Users
                .Include(c => c.Customer)
                .FirstAsync(c => c.Id == userId);

            return new UserProfileDto
            {
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Address = user.Customer?.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<ResetPasswordToken> GetResetPasswordToken(string email)
        {
            var user = await _userMgr.FindByEmailAsync(email)
                ?? throw new RepositoryEntityNotFoundException($"No such user with email {email}");

            return new ResetPasswordToken
            {
                Token = await _userMgr.GeneratePasswordResetTokenAsync(user)
            };
        }

        public async Task ResetPassword(ResetPasswordDto data)
        {
            var user = await _userMgr.FindByEmailAsync(data.Email)
                ?? throw new RepositoryEntityNotFoundException($"No such user with email {data.Email}");

            var result = await _userMgr.ResetPasswordAsync(user, data.ResetToken, data.NewPassword);
            if (!result.Succeeded)
            {
                throw new IdentityException("Could not change the password", StatusCodes.Status400BadRequest, result.Errors);
            }
        }

        public async Task UpdateProfile(Guid userId, UserUpdateProfileDto data)
        {
            var user = await _authContext.Users
                .Include(c => c.Customer)
                .FirstAsync(c => c.Id == userId);

            user.FullName = data.FullName;
            user.PhoneNumber = data.PhoneNumber;
            user.BirthDate = data.BirthDate;
            user.Gender = data.Gender;
            user.Customer!.Address = data.Address;

            await _userMgr.UpdateAsync(user);

        }
    }
}
