using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Exceptions;
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

        public async Task AddDishesToMenu(Guid manager, Guid menuId, List<Guid> dishesIds)
        {
            var restaurant = await _backendContext.Restaurants
                .Include(r => r.Menus)
                    .ThenInclude(m => m.Dishes)
                .Where(r => r.Managers.Contains(manager))
                .Select(r => new { r.Menus, r.Dishes })
                .FirstAsync();

            var menu = restaurant.Menus
                .First(m => m.Id == menuId);

            var dishes = restaurant.Dishes
                .IntersectBy(dishesIds, d => d.Id)
                .Distinct();

            var uncatched = dishesIds.Where(d => !dishes.Select(x => x.Id).Contains(d));
            if (!uncatched.IsNullOrEmpty())
            {
                throw new RepositoryEntityNotFoundException($"Some dishes do not exist", new {NonExistingDishes = uncatched});
            }

            dishes = dishes.Except(menu.Dishes);

            if (dishes.IsNullOrEmpty())
            {
                throw new RepositoryEntityAlreadyExistsException("All of those dishes are already in the menu");
            }

            menu.Dishes.AddRange(dishes);

            await _backendContext.SaveChangesAsync();
        }

        public async Task CreateMenu(Guid manager, CreateMenuDto data)
        {
            var restaurant = await _backendContext.Restaurants
                .Include(r => r.Menus)
                .FirstAsync(r => r.Managers.Contains(manager));

            restaurant.Menus.Add(new Menu { Name = data.Name });

            await _backendContext.SaveChangesAsync();
        }

        public async Task<PagedRestaurantsDto> GetAllRestaurants(int page, string? name = null)
        {
            var query = _backendContext.Restaurants
                .Include(r => r.Menus)
                    .ThenInclude(m => m.Dishes)
                .Where(r => name == null || r.NormalizedName.Contains(name.ToUpper().Normalize()));

            var response = new PagedRestaurantsDto
            {
                PageInfo = new PageInfo(query.Count(), _RestaurantPageSize, page)
            };

            var restaurants = await query
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
            var restaurant = await _backendContext.Menus
                .Where(m => m.Id == menuId)
                .Select(m => m.Restaurant.Id)
                .FirstAsync();

            filters.Menu = menuId;

            return await GetActiveRestaurantDishes(restaurant, page, filters);
        }

        public async Task<PagedDishesDto> GetActiveRestaurantDishes(Guid restaurantId, int page, DishFilters filters)
        {
            var query = _backendContext.Menus
                .Include(m => m.Dishes)
                    .ThenInclude(d => d.Ratings)
                .Where(m => m.Restaurant.Id == restaurantId && m.IsActive == true);

            if (filters.Menu != null)
            {
                query = query.Where(m => m.Id == filters.Menu);
            }

            var filteredDishes = await query
                .SelectMany(m => m.Dishes)
                .ToListAsync();

            filteredDishes = filteredDishes
                .FilterByCategories(filters.Categories)
                .FilterByVegetarian(filters.IsVegetarian)
                .SortByType(filters.SortBy)
                .ToList();

            var response = new PagedDishesDto();
            foreach (var dish in filteredDishes)
            {
                response.Dishes.Add(new DishShortDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Rating = dish.Rating,
                    IsVegeterian = dish.IsVegeterian,
                    Photo = dish.Photo,
                    Category = dish.Category,
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
                    && (name == null || m.NormalizedName.Contains(name.ToUpper().Normalize()))
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
            var restaurant = await _backendContext.Restaurants
                .Include(r => r.Menus)
                    .ThenInclude(m => m.Dishes)
                .Where(r => r.Managers.Contains(manager))
                .Select(r => new { r.Menus, r.Dishes })
                .FirstOrDefaultAsync()
                ?? throw new PersonUnemployedException($"{manager} is unemployed");

            var menu = restaurant.Menus
                .FirstOrDefault(m => m.Id == menuId)
                ?? throw new RepositoryEntityNotFoundException($"No such menu w/ id = {menuId}");

            var dishes = restaurant.Dishes
                .IntersectBy(dishesIds, d => d.Id);

            menu.Dishes.RemoveAll(dishes.Contains);

            await _backendContext.SaveChangesAsync();
        }

        public async Task<PagedDishesDto> GetAllRestaurantDishes(Guid manager, int page)
        {
            var restaurant = await _backendContext.Restaurants
                .Include(m => m.Dishes)
                    .ThenInclude(d => d.Ratings)
                .FirstOrDefaultAsync(m => m.Managers.Contains(manager))
                ?? throw new PersonUnemployedException($"{manager} is unemployed");

            var dishes = restaurant.Dishes;

            var response = new PagedDishesDto();
            foreach (var dish in dishes)
            {
                response.Dishes.Add(new DishShortDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Rating = dish.Rating,
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
            var restaurant = await _backendContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefaultAsync(r => r.Managers.Contains(manager))
                ?? throw new PersonUnemployedException($"{manager} is unemployed");

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
            var menus = await _backendContext.Restaurants.Include(r => r.Menus).ThenInclude(m => m.Dishes)
                .Where(r => r.Managers.Contains(manager))
                .Select(r => r.Menus)
                .FirstOrDefaultAsync()
                ?? throw new PersonUnemployedException($"{manager} is unemployed"); ;

            var response = new PagedMenusDto
            {
                PageInfo = new PageInfo(menus.Count, _MenusPageSize, page)
            };

            menus = menus
                .Skip((page - 1) * _MenusPageSize)
                .Take(_MenusPageSize)
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
            var restaurant = await _backendContext.Restaurants
                .Include(r => r.Dishes)
                .FirstAsync(r => r.Managers.Contains(manager))
                ?? throw new PersonUnemployedException($"{manager} is unemployed");

            restaurant.Dishes.Remove(
                restaurant.Dishes.First(d => d.Id == dishId)
                );

            await _backendContext.SaveChangesAsync();
        }

        public async Task<MenuInfo> GetMenuDetails(Guid manager, Guid menuId, int dishPage)
        {
            var restaurant = await _backendContext.Restaurants
                .Include(r => r.Menus)
                    .ThenInclude(m => m.Dishes)
                        .ThenInclude(d => d.Ratings)
                .FirstAsync(r => r.Managers.Contains(manager))
                ?? throw new PersonUnemployedException($"{manager} is unemployed");

            var menu = restaurant.Menus
                .FirstOrDefault(m => m.Id == menuId)
                ?? throw new RepositoryEntityNotFoundException($"No such menu w/ id = {menuId}");

            var dishesInfo = new PagedDishesDto
            {
                PageInfo = new PageInfo(menu.Dishes.Count, _DishPageSize, dishPage)
            };

            foreach (var dish in menu.Dishes)
            {
                dishesInfo.Dishes.Add(new DishShortDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Rating = dish.Rating,
                    IsVegeterian = dish.IsVegeterian,
                    Photo = dish.Photo,
                    Category = dish.Category
                });
            }

            var response = new MenuInfo
            {
                Name = menu.Name,
                IsActive = menu.IsActive,
                Dishes = dishesInfo
            };

            return response;
        }

        public async Task UpdateMenu(Guid manager, Guid menuId, UpdateMenuDto data)
        {
            var menu = (
                await _backendContext.Restaurants.Include(r => r.Menus).ThenInclude(m => m.Dishes)
                    .Where(r => r.Managers.Contains(manager))
                    .Select(r => r.Menus)
                    .FirstOrDefaultAsync()
                    ?? throw new PersonUnemployedException($"{manager} is unemployed")
                )
                .FirstOrDefault(m => m.Id == menuId)
                ?? throw new RepositoryEntityNotFoundException($"No such menu w/ id = {menuId}");

            menu.Name = data.Name;
            menu.IsActive = data.IsActive;

            await _backendContext.SaveChangesAsync();
        }
        public async Task ChangeMenuVisibility(Guid manager, Guid menuId, bool isActive = true)
        {
            var menu = (
                await _backendContext.Restaurants.Include(r => r.Menus).ThenInclude(m => m.Dishes)
                    .Where(r => r.Managers.Contains(manager))
                    .Select(r => r.Menus)
                    .FirstAsync()
                    ?? throw new PersonUnemployedException($"{manager} is unemployed")
                )
                .FirstOrDefault(m => m.Id == menuId)
                ?? throw new RepositoryEntityNotFoundException($"No such menu w/ id = {menuId}");

            menu.IsActive = isActive;
            await _backendContext.SaveChangesAsync();
        }

        public async Task UpdateDish(Guid manager, Guid dishId, CreateDishDto data)
        {
            var dish = (
                await _backendContext.Restaurants.Include(r => r.Menus).ThenInclude(m => m.Dishes)
                    .Where(r => r.Managers.Contains(manager))
                    .Select(r => r.Dishes)
                    .FirstAsync()
                    ?? throw new PersonUnemployedException($"{manager} is unemployed")
                )
                .FirstOrDefault(d => d.Id == dishId)
                ?? throw new RepositoryEntityNotFoundException($"No such dish w/ id = {dishId}");

            dish.Name = data.Name;
            dish.Description = data.Description;
            dish.IsVegeterian = data.IsVegetarian;
            dish.Photo = data.Photo;
            dish.Category = data.FoodCategory;
            dish.Price = data.Price;

            await _backendContext.SaveChangesAsync();

        }
        public async Task DeleteMenu(Guid manager, Guid menuId)
        {
            var restaurant = await _backendContext.Restaurants
                .Include(r => r.Menus)
                .FirstAsync(r => r.Managers.Contains(manager));

            restaurant.Menus.Remove(
                restaurant.Menus.First(d => d.Id == menuId)
                );

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

        public static ICollection<Dish> SortByType(this ICollection<Dish> collection, DishSortingType? sortBy)
        {
            return (sortBy switch
            {
                DishSortingType.RATING_DESCENDING => collection.OrderByDescending(d => d.Rating),
                DishSortingType.NAME_DESCENDING => collection.OrderByDescending(d => d.Name),
                DishSortingType.NAME_ASCENDING => collection.OrderBy(d => d.Name),
                DishSortingType.PRICE_DESCENDING => collection.OrderByDescending(d => d.Price),
                DishSortingType.PRICE_ASCENDING => collection.OrderBy(d => d.Price),
                DishSortingType.RATING_ASCENDING => collection.OrderBy(d => d.Rating),
                _ => collection.OrderBy(d => d.Name),
            }).ToList();
        }
    }
}
