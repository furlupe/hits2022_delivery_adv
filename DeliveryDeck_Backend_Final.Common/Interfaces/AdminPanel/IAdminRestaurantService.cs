

using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel
{
    public interface IAdminRestaurantService
    {
        public Task CreateRestaurant(RestaurantDto data);
        public Task<PagedRestaurantsDto> GetRestaurants(int page = 1, string? name = null);
        public Task DeleteRestaurant(Guid id);
    }
}
