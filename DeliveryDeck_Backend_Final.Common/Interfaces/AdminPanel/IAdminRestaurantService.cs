

using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel
{
    public interface IAdminRestaurantService
    {
        public Task CreateRestaurant(RestaurantShortDto data);
        public Task<PagedRestaurantsDto> GetRestaurants(int page = 1, string? name = null);
        public Task<RestaurantDto> GetRestaurantInfo(Guid id);
        public Task DeleteRestaurant(Guid id);
    }
}
