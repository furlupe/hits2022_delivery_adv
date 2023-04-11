using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IRestaurantService
    {
        Task<PagedRestaurantsDto> GetAllRestaurants(int page, string? name = null);
        Task<PagedDishesDto> GetRestaurantDishes(Guid restaurantId, int page, DishFilters filters);
        Task<PagedMenusDto> GetRestaurantMenus(Guid restaurantId, int page, string? name = null);
        Task<PagedDishesDto> GetMenuDishes(Guid menuId, int page, DishFilters filters);
    }
}
