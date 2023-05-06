﻿

using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel
{
    public interface IAdminRestaurantService
    {
        public Task CreateRestaurant(RestaurantShortDto data);
        public Task<PagedRestaurantsDto> GetRestaurants(int page = 1, string? name = null);
        public Task<RestaurantDto> GetRestaurantInfo(Guid id, int page = 1, List<RoleType>? staffRoles = default);
        public Task AddStaffToRestaurant(Guid restaurantId, StaffDto data);
        public Task DeleteRestaurant(Guid id);
    }
}
