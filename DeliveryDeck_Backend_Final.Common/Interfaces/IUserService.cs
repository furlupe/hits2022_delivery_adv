using DeliveryDeck_Backend_Final.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfile(Guid userId);
        Task UpdateProfile(Guid userId, UserUpdateProfileDto data);
        Task ChangePassword(Guid userId, ChangePasswordDto passwords);
    }
}
