using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IRestaurantService
    {
        Task<PagedRestaurantsDto> GetAllRestaurants(int page, string? name = null);
        Task<PagedDishesDto> GetActiveRestaurantDishes(Guid restaurantId, int page, DishFilters filters);
        Task<PagedMenusDto> GetActiveRestaurantMenus(Guid restaurantId, int page, string? name = null);
        Task<PagedDishesDto> GetActiveMenuDishes(Guid menuId, int page, DishFilters filters);

        Task CreateMenu(Guid manager, CreateMenuDto data);
        Task<PagedMenusDto> GetMenus(Guid manager, int page);
        Task<MenuInfo> GetMenuDetails(Guid manager, Guid menuId, int dishPage);
        Task UpdateMenu(Guid manager, Guid menuId, UpdateMenuDto data);
        Task ChangeMenuVisibility(Guid manager, Guid menuId, bool isActive = true);
        Task DeleteMenu(Guid manager, Guid menuId);


        Task AddDishesToMenu(Guid manager, Guid menuId, List<Guid> dishesIds);
        Task RemoveDishesFromMenu(Guid manager, Guid menuId, List<Guid> dishesIds);

        Task<PagedDishesDto> GetAllRestaurantDishes(Guid manager, int page);
        Task AddDishToRestaurant(Guid manager, CreateDishDto data);
        Task RemoveDishFromRestaurant(Guid manager, Guid dishId);
        Task UpdateDish(Guid manager, Guid dishId, CreateDishDto data);
    }
}
