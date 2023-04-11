using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly BackendContext _backendContext;
        private readonly int _RestaurantPageSize = 2;
        private readonly int _MenusPageSize = 1;
        private readonly int _DishPageSize = 1;
        public RestaurantService(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }
        public async Task<PagedRestaurantsDto> GetAllRestaurants(int page, string? name = null)
        {
            var response = new PagedRestaurantsDto
            {
                PageInfo = new PageInfo(_backendContext.Restaurants.Count(), _RestaurantPageSize, page)
            };

            var restaurants = await _backendContext.Restaurants
                .Where(r => name == null || r.Name.StartsWith(name))
                .Skip((page - 1) * _RestaurantPageSize)
                .Take(_RestaurantPageSize)
                .ToListAsync();

            foreach (var restaurant in restaurants)
            {
                response.Restaurants.Add(new RestaurantShortDto
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name
                });
            }

            return response;
        }

        public async Task<PagedDishesDto> GetMenuDishes(Guid menuId, int page, Filters filters)
        {
            var menu = await _backendContext.Menus
                .Where(m => m.Id == menuId)
                .Select(m => new { m.Restaurant.Id, m.Name })
                .SingleAsync();

            filters.Menu = menu.Name;

            return await GetRestaurantDishes(menu.Id, page, filters);
        }

        public async Task<PagedDishesDto> GetRestaurantDishes(Guid restaurantId, int page, Filters filters)
        {
            var query = _backendContext.Menus
                .Include(m => m.Dishes)
                    .ThenInclude(d => d.Ratings)
                .Where(m => m.Restaurant.Id == restaurantId && m.IsActive == true);

            if (filters.Menu != null)
            {
                query = query.Where(m => m.Name == filters.Menu);
            }

            var menuDishes = await query
                .Select(m => m.Dishes)
                .ToListAsync();

            var filteredDishes = new List<Dish>();

            foreach (var menu in menuDishes)
            {
                filteredDishes.AddRange(menu);
            }

            filteredDishes = filteredDishes
                .FilterByCategories(filters.Categories)
                .FilterByVegetarian(filters.IsVegetarian)
                .SortByType(filters.SortBy)
                .ToList();

            var response = new PagedDishesDto();
            foreach (var dish in filteredDishes)
            {
                response.Dishes.Add(new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Rating = dish.Rating,
                    Description = dish.Description,
                    IsVegeterian = dish.IsVegeterian,
                    Photo = dish.Photo,
                    Category = dish.Category
                });
            }

            response.PageInfo = new PageInfo(response.Dishes.Count, _DishPageSize, page);

            response.Dishes = response.Dishes
                .Skip((page - 1) * _DishPageSize)
                .Take(_DishPageSize)
                .ToList();

            return response;
        }

        public async Task<PagedMenusDto> GetRestaurantMenus(Guid restaurantId, int page, string? name = null)
        {
            var menus = await _backendContext.Menus
                .Where(m => 
                    m.Restaurant.Id == restaurantId 
                    && (name == null || m.Name.StartsWith(name))
                    && m.IsActive == true
                )
                .Select(m => new { m.Id, m.Name })
                .ToListAsync();

            var response = new PagedMenusDto
            {
                PageInfo = new PageInfo(menus.Count, _MenusPageSize, page)
            };

            menus = menus
                .Skip((page - 1) * _RestaurantPageSize)
                .Take(_RestaurantPageSize)
                .ToList();

            foreach (var menu in menus)
            {
                response.Menus.Add(new MenuDto
                {
                    Id = menu.Id,
                    Name = menu.Name
                });
            }

            return response;
        }
    }

    public static class DishEnumerableExtensions
    {
        public static ICollection<Dish> FilterByCategories(this ICollection<Dish> collection, ICollection<FoodCategory> categories)
        {
            if (!categories.IsNullOrEmpty())
            {
                collection = collection
                    .Where(d => categories.Contains(d.Category))
                    .ToList();
            }

            return collection;
        }
        public static ICollection<Dish> FilterByVegetarian(this ICollection<Dish> collection, bool? isVegetarian = null)
        {
            if (isVegetarian != null)
            {
                collection = collection
                    .Where(d => d.IsVegeterian == isVegetarian)
                    .ToList();
            }

            return collection;
        }

        public static ICollection<Dish> SortByType(this ICollection<Dish> collection, SortingType? sortBy)
        {
            return (sortBy switch
            {
                SortingType.RATING_DESCENDING => collection.OrderByDescending(d => d.Rating),
                SortingType.NAME_DESCENDING => collection.OrderByDescending(d => d.Name),
                SortingType.NAME_ASCENDING => collection.OrderBy(d => d.Name),
                SortingType.PRICE_DESCENDING => collection.OrderByDescending(d => d.Price),
                SortingType.PRICE_ASCENDING => collection.OrderBy(d => d.Price),
                SortingType.RATING_ASCENDING => collection.OrderBy(d => d.Rating),
                _ => collection.OrderBy(_ => _),
            }).ToList();
        }
    }
}
