using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.BLL.Services
{
    public class AdminRestaurantService : IAdminRestaurantService
    {
        private readonly AuthContext _authContext;
        private readonly BackendContext _backendContext;
        private const int _RestaurantPageSize = 1;

        public AdminRestaurantService(AuthContext authContext, BackendContext backendContext)
        {
            _authContext = authContext;
            _backendContext = backendContext;
        }

        public async Task CreateRestaurant(RestaurantDto data)
        {
            if (await _backendContext.Restaurants.AnyAsync(r => r.Name == data.Name))
            {
                throw new BadHttpRequestException("Name is already taken");
            }

            var existingManagers = await _authContext.Managers
                .Where(m => data.Managers.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync();

            var existingCooks = await _authContext.Cooks
                .Where(m => data.Cooks.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync();

            if (data.Managers.Except(existingManagers).Any())
            {
                throw new BadHttpRequestException("No such manager");
            }

            if (data.Cooks.Except(existingCooks).Any())
            {
                throw new BadHttpRequestException("No such cook");
            }

            var availableManagers = new List<Guid>();
            var availableCooks = new List<Guid>();
            foreach(var manager in existingManagers)
            {

                if (await _backendContext.Restaurants.AllAsync(r => !r.Managers.Contains(manager)))
                {
                    availableManagers.Add(manager);
                }

                throw new BadHttpRequestException("Manager taken");
            }

            foreach(var cook in existingCooks)
            {
                if (await _backendContext.Restaurants.AllAsync(r => !r.Cooks.Contains(cook)))
                {
                    availableCooks.Add(cook);
                }

                throw new BadHttpRequestException("Cook taken");
            }

            await _backendContext.Restaurants.AddAsync(new Restaurant
            {
                Name = data.Name,
                Managers = availableManagers,
                Cooks = availableCooks
            });

            await _backendContext.SaveChangesAsync();
        }

        public async Task<PagedRestaurantsDto> GetRestaurants(int page = 1, string? name = null)
        {
            var query = _backendContext.Restaurants
                .Include(r => r.Menus)
                    .ThenInclude(m => m.Dishes)
                .Where(r => name == null || r.NormalizedName.Contains(name));

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
    }
}
