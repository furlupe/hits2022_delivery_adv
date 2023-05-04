using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel
{
    public interface IAdminUserService
    {
        public Task<PagedUsersDto> GetUsers(int page = 1);
        public Task DeleteUser(Guid id);
        public Task<UserExtendedDto> GetUserInfo(Guid id);
    }
}
