using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Exceptions;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DeliveryDeck_Backend_Final.Auth.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userMgr;

        public UserService(UserManager<AppUser> userMgr)
        {
            _userMgr = userMgr;
        }

        public async Task<UserProfileDto> GetProfile(Guid userId)
        {
            var user = await _userMgr.FindByIdAsync(userId.ToString());
            return new UserProfileDto
            {
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<ResetPasswordToken> GetResetPasswordToken(string email)
        {
            var user = await _userMgr.FindByEmailAsync(email)
                ?? throw new BadHttpRequestException(string.Format($"No such user with email {0}", email));

            return new ResetPasswordToken {
                Token = await _userMgr.GeneratePasswordResetTokenAsync(user) 
            };
        }

        public async Task ResetPassword(ResetPasswordDto data)
        {
            var user = await _userMgr.FindByEmailAsync(data.Email)
                ?? throw new BadHttpRequestException(string.Format($"No such user with email {0}", data.Email));

            var result = await _userMgr.ResetPasswordAsync(user, data.ResetToken, data.NewPassword);
            if(!result.Succeeded)
            {
                throw new IdentityException("Could not change the password", StatusCodes.Status400BadRequest, result.Errors);
            }
        }

        public async Task UpdateProfile(Guid userId, UserUpdateProfileDto data)
        {
            var user = await _userMgr.FindByIdAsync(userId.ToString());

            user.FullName = data.FullName;
            user.PhoneNumber = data.PhoneNumber;
            user.BirthDate = data.BirthDate;
            user.Gender = data.Gender;
            user.Address = data.Address;

            await _userMgr.UpdateAsync(user);

        }
    }
}
