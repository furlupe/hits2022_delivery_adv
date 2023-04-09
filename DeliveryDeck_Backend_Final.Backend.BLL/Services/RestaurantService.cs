using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces;
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
            var pagesAmount = (int)Math.Ceiling((double)_backendContext.Restaurants.Count() / _RestaurantPageSize);

            if (pagesAmount < page || page < 1)
            {
                throw new BadHttpRequestException("Page out of range");
            }

            var query = _backendContext.Restaurants
                .Where(r => name == null || r.Name.StartsWith(name))
                .Skip((page - 1) * _RestaurantPageSize)
                .Take(_RestaurantPageSize);

            var restaurants = await query.ToListAsync();

            var response = new PagedRestaurantsDto
            {
                PageInfo = new PageInfo
                {
                    CurrentPage = page,
                    Pages = pagesAmount,
                    PageSize = restaurants.Count()
                }
            };

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
            // 
            var query = _backendContext.Menus
                .Include(m => m.Dishes)
                    .ThenInclude(d => d.Ratings)
                .Where(m => m.Restaurant.Id == restaurantId && m.IsActive == true);

            if (filters.Menu != null)
            {
                query = query.Where(m => m.Name == filters.Menu);
            }

            var menus = await query.Select(m => m.Dishes).ToListAsync();

            var response = new PagedDishesDto();
            var filteredDishes = new List<Dish>();

            foreach (var menu in menus)
            {
                filteredDishes.AddRange(menu);
            }

            filteredDishes = filteredDishes
                .FilterByCategories(filters.Categories)
                .FilterByVegetarian(filters.IsVegetarian)
                .ToList();

            foreach (var dish in filteredDishes)
            {
                response.Dishes.Add(new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Rating = dish.Ratings.IsNullOrEmpty() ? 0 : dish.Ratings.Average(r => r.Value),
                    Description = dish.Description,
                    IsVegeterian = dish.IsVegeterian,
                    Photo = dish.Photo,
                    Category = dish.Category
                });
            }

            response.Dishes = response.Dishes
                .SortBy(filters.SortBy)
                .ToList();

            var pagesAmount = (int)Math.Ceiling((double)response.Dishes.Count() / _DishPageSize);

            if (pagesAmount < page || page < 1)
            {
                throw new BadHttpRequestException("Page out of range");
            }

            response.Dishes = response.Dishes
                .Skip((page - 1) * _DishPageSize)
                .Take(_DishPageSize)
                .ToList();

            response.PageInfo = new PageInfo
            {
                CurrentPage = page,
                Pages = pagesAmount,
                PageSize = response.Dishes.Count
            };

            return response;
        }

        public async Task<PagedMenusDto> GetRestaurantMenus(Guid restaurantId, int page, string? name = null)
        {
            var menus = await _backendContext.Menus
                .Where(m => m.Restaurant.Id == restaurantId && (name == null || m.Name.StartsWith(name)))
                .Select(m => new { m.Id, m.Name })
                .ToListAsync();

            var pagesAmount = (int)Math.Ceiling((double)menus.Count / _MenusPageSize);

            if (pagesAmount < page || page < 1)
            {
                throw new BadHttpRequestException("Page out of range");
            }

            menus = menus
                .Skip((page - 1) * _RestaurantPageSize)
                .Take(_RestaurantPageSize)
                .ToList();

            var response = new PagedMenusDto
            {
                PageInfo = new PageInfo
                {
                    CurrentPage = page,
                    Pages = pagesAmount,
                    PageSize = menus.Count
                }
            };

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
    }

    public static class DishShortDtoEnumerableExtensions
    {
        public static ICollection<DishDto> SortBy(this ICollection<DishDto> collection, SortingType? sortBy)
        {
            if (sortBy != null)
            {
                var sortedCollection = (IEnumerable<DishDto>)collection;
                switch (sortBy)
                {
                    case SortingType.NAME_DESCENDING:
                        sortedCollection = collection
                            .OrderByDescending(d => d.Name);
                        break;

                    case SortingType.NAME_ASCENDING:
                        sortedCollection = collection
                            .OrderBy(d => d.Name);
                        break;

                    case SortingType.PRICE_ASCENDING:
                        sortedCollection = collection
                            .OrderBy(d => d.Price);
                        break;

                    case SortingType.PRICE_DESCENDING:
                        sortedCollection = collection
                            .OrderByDescending(d => d.Price);
                        break;

                    case SortingType.RATING_ASCENDING:
                        sortedCollection = collection
                            .OrderBy(d => d.Rating);
                        break;

                    case SortingType.RATING_DESCENDING:
                        sortedCollection = collection
                            .OrderByDescending(d => d.Rating);
                        break;
                }

                collection = sortedCollection.ToList();
            }

            return collection;
        }
    }
}
