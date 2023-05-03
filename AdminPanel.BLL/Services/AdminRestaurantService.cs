using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly BackendContext _backendContext;
        private const int _RestaurantPageSize = 3;

        public AdminRestaurantService(AuthContext authContext, BackendContext backendContext, IMapper mapper)
        {
            _authContext = authContext;
            _backendContext = backendContext;
            _mapper = mapper;
        }

        public async Task CreateRestaurant(RestaurantShortDto data)
        {
            if (await _backendContext.Restaurants.AnyAsync(r => r.Name == data.Name))
            {
                throw new BadHttpRequestException("Name is already taken");
            }

            await _backendContext.Restaurants.AddAsync(new Restaurant
            {
                Name = data.Name
            });

            await _backendContext.SaveChangesAsync();
        }

        public async Task<RestaurantDto> GetRestaurantInfo(Guid id)
        {
            var restaurant = await _backendContext.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new BadHttpRequestException("No such restaurant");

            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public async Task<PagedRestaurantsDto> GetRestaurants(int page = 1, string? name = null)
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

        public async Task DeleteRestaurant(Guid id)
        {
            var restaurant = await _backendContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new BadHttpRequestException("No such restaurant");

            _backendContext.Restaurants.Remove(restaurant);
            try
            {
                await _backendContext.SaveChangesAsync();
            }
            catch
            {
                throw new BadHttpRequestException("what the fuck");
            }
        }

    }
}
