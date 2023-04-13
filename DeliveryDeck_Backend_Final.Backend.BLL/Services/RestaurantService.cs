using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;

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

        public async Task AddDishesToMenu(Guid manager, Guid menuId, List<Guid> dishesIds)
        {
            var restaurant = await GetRestaurantByManager(manager);
            var menu = restaurant.Menus.FirstOrDefault(m => m.Id == menuId)
                ?? throw new BadHttpRequestException("", StatusCodes.Status404NotFound);

            var dishes = restaurant.Dishes
                .IntersectBy(dishesIds, d => d.Id)
                .Except(menu.Dishes);

            var uncatched = dishesIds.Where(d => !dishes.Select(x => x.Id).Contains(d)).ToList();
            if (!uncatched.IsNullOrEmpty())
            {
                throw new BadHttpRequestException("Some dishes do not exist");
            }

            menu.Dishes.AddRange(dishes);

            await _backendContext.SaveChangesAsync();
        }

        public async Task CreateMenu(Guid manager, CreateMenuDto data)
        {
            var restaurant = await GetRestaurantByManager(manager);
            var menu = new Menu { Name = data.Name };

            restaurant.Menus.Add(new Menu { Name = data.Name });

            await _backendContext.SaveChangesAsync();
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

        public async Task<PagedDishesDto> GetActiveMenuDishes(Guid menuId, int page, DishFilters filters)
        {
            var menu = await _backendContext.Menus
                .Where(m => m.Id == menuId)
                .Select(m => new { m.Restaurant.Id, m.Name })
                .FirstOrDefaultAsync()
                ?? throw new BadHttpRequestException("No such menu", StatusCodes.Status404NotFound);

            filters.Menu = menu.Name;

            return await GetActiveRestaurantDishes(menu.Id, page, filters);
        }

        public async Task<PagedDishesDto> GetActiveRestaurantDishes(Guid restaurantId, int page, DishFilters filters)
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

        public async Task<PagedMenusDto> GetActiveRestaurantMenus(Guid restaurantId, int page, string? name = null)
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

        public async Task RemoveDishesFromMenu(Guid manager, Guid menuId, List<Guid> dishesIds)
        {
            var restaurant = await GetRestaurantByManager(manager);
            var menu = restaurant.Menus.FirstOrDefault(m => m.Id == menuId)
                ?? throw new BadHttpRequestException("", StatusCodes.Status404NotFound);

            var dishes = restaurant.Dishes
                .IntersectBy(dishesIds, d => d.Id);

            menu.Dishes.RemoveAll(dishes.Contains);

            await _backendContext.SaveChangesAsync();
        }

        public async Task<PagedDishesDto> GetAllRestaurantDishes(Guid manager, int page)
        {
            var dishes = await _backendContext.Restaurants
                .Include(m => m.Dishes)
                    .ThenInclude(d => d.Ratings)
                .Where(m => m.Managers.Contains(manager))
                .SelectMany(x => x.Dishes)
                .ToListAsync();

            var response = new PagedDishesDto();
            foreach (var dish in dishes)
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

        public async Task AddDishToRestaurant(Guid manager, CreateDishDto data)
        {
            var restaurant = await GetRestaurantByManager(manager);

            restaurant.Dishes.Add(new Dish
            {
                Name = data.Name,
                Description = data.Description,
                IsVegeterian = data.IsVegetarian,
                Photo = data.Photo,
                Category = data.FoodCategory,   
                Price = data.Price
            });

            await _backendContext.SaveChangesAsync();
        }

        public async Task<PagedMenusDto> GetMenus(Guid manager, int page)
        {
            var restaurant = await GetRestaurantByManager(manager);

            var menus = restaurant.Menus;
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

        public async Task RemoveDishFromRestaurant(Guid manager, Guid dishId)
        {
            var restaurant = await GetRestaurantByManager(manager);

            restaurant.Dishes.Remove(
                restaurant.Dishes.FirstOrDefault(d => d.Id == dishId)
                ?? throw new BadHttpRequestException("No such dish", StatusCodes.Status404NotFound)
                );
            await _backendContext.SaveChangesAsync();
        }

        public async Task<MenuInfo> GetMenuDetails(Guid manager, Guid menuId, int dishPage)
        {
            var restaurant = await GetRestaurantByManager(manager);
            var menu = restaurant.Menus.FirstOrDefault(m => m.Id == menuId)
                ?? throw new BadHttpRequestException("Menu does not exist", StatusCodes.Status404NotFound);

            var dishesInfo = new PagedDishesDto
            {
                PageInfo = new PageInfo(menu.Dishes.Count, _DishPageSize, dishPage)
            };

            foreach(var dish in menu.Dishes)
            {
                dishesInfo.Dishes.Add(new DishDto
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

            var response = new MenuInfo();
            response.Name = menu.Name;
            response.IsActive = menu.IsActive;
            response.Dishes = dishesInfo;

            return response;
        }

        private Task<Restaurant> GetRestaurantByManager(Guid managerId)
        {
            return _backendContext.Restaurants
                .Include(r => r.Menus)
                    .ThenInclude(m => m.Dishes)
                .Include(d => d.Dishes)
                    .ThenInclude(d => d.Ratings)
                .FirstAsync(r => r.Managers.Contains(managerId));
        }

        public async Task UpdateMenu(Guid manager, Guid menuId, UpdateMenuDto data)
        {
            var menu = (await GetRestaurantByManager(manager))
                .Menus.FirstOrDefault(m => m.Id == menuId)
                ?? throw new BadHttpRequestException("No such menu", StatusCodes.Status404NotFound);

            menu.Name = data.Name;
            menu.IsActive = data.IsActive;

            await _backendContext.SaveChangesAsync();
        }

        public async Task UpdateDish(Guid manager, Guid dishId, CreateDishDto data)
        {
            var dish = (await GetRestaurantByManager(manager))
                .Dishes.FirstOrDefault(d => d.Id == dishId)
                ?? throw new BadHttpRequestException("No such dish", StatusCodes.Status404NotFound);

            dish.Name = data.Name;
            dish.Description = data.Description;
            dish.IsVegeterian = data.IsVegetarian;
            dish.Photo = data.Photo;
            dish.Category = data.FoodCategory;
            dish.Price = data.Price;

            await _backendContext.SaveChangesAsync();

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
