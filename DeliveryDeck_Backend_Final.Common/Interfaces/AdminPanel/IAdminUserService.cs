﻿using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel
{
    public interface IAdminUserService
    {
        public Task<PagedUsersDto> GetUsers(int page = 1);
        public Task<PagedAvailableStaffDto> GetAvailableStaff(int page = 1);
        public Task CreateUser(UserCreateDto data);
        public Task UpdateUser(Guid id, UserUpdateDto data);
        public Task DeleteUser(Guid id);
        public Task BanUser(Guid id);
        public Task UnbanUser(Guid id);
        public Task<UserExtendedDto> GetUserInfo(Guid id);
    }
}
