using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Interfaces;
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
