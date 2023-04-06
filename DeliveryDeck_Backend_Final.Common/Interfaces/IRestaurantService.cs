using DeliveryDeck_Backend_Final.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.Interfaces
{
    public interface IRestaurantService
    {
        Task<PagedRestaurantsDto> GetAllRestaurants(int page, string? name = null);
        Task<PagedDishesDto> GetRestaurantDishes(Guid restaurantId, int page, Filters filters);
        Task<PagedMenusDto> GetRestaurantMenus(Guid restaurantId, int page, string? name = null);
        Task<PagedDishesDto> GetMenuDishes(Guid menuId, int page, Filters filters);
    }
}
