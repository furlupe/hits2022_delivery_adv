using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel
{
    public interface IAdminUserService
    {
        public Task<PagedUsersDto> GetUsers(int page = 1, List<RoleType>? roles = default, bool availableOnly = false);
        public Task CreateUser(UserCreateDto data);
        public Task DeleteUser(Guid id);
        public Task<UserExtendedDto> GetUserInfo(Guid id);
    }
}
