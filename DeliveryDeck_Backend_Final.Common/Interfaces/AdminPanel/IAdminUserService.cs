using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel
{
    public interface IAdminUserService
    {
        public Task<PagedUsersDto> GetUsers(int page = 1);
        public Task<PagedAvailableStaffDto> GetAvailableStaff(int page = 1);
        public Task CreateUser(UserCreateDto data);
        public Task UpdateUser(Guid id, UserUpdateDto data);
        public Task DeleteUser(Guid id);
        public Task<UserExtendedDto> GetUserInfo(Guid id);
    }
}
