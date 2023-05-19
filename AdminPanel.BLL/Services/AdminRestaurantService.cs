using AutoMapper;
using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task CreateRestaurant(RestaurantCreateDto data)
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

        public async Task<RestaurantDto> GetRestaurantInfo(Guid id, int page = 1, List<RoleType>? staffRoles = default)
        {
            var restaurant = await _backendContext.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new BadHttpRequestException("No such restaurant");

            var cooks = new List<StaffDto>();
            var managers = new List<StaffDto>();

            if (staffRoles.IsNullOrEmpty() || staffRoles.Contains(RoleType.Cook))
            {
                cooks = (
                        await _authContext.Cooks
                        .Where(x => restaurant.Cooks.Contains(x.Id))
                        .Select(x => x.User)
                        .ToListAsync()
                    )
                    .Select(_mapper.Map<StaffDto>)
                    .ToList();

                cooks.ForEach(x => x.Role = RoleType.Cook);
            }

            if (staffRoles.IsNullOrEmpty() || staffRoles.Contains(RoleType.Manager))
            {
                managers = (
                        await _authContext.Managers
                        .Where(x => restaurant.Managers.Contains(x.Id))
                        .Select(x => x.User)
                        .ToListAsync()
                    )
                    .Select(_mapper.Map<StaffDto>)
                    .ToList();

                managers.ForEach(x => x.Role = RoleType.Manager);
            }

            var staff = managers.Concat(cooks);

            return new RestaurantDto
            {
                Id = id,
                Name = restaurant.Name,
                Staff = new PagedStaffDto
                {
                    Staff = staff.Skip((page - 1) * _RestaurantPageSize).Take(_RestaurantPageSize).ToList(),
                    PageInfo = new PageInfo(staff.Count(), _RestaurantPageSize, page)
                }
            };
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
                response.Restaurants.Add(_mapper.Map<RestaurantShortDto>(restaurant));
            }

            return response;
        }

        public async Task DeleteRestaurant(Guid id)
        {
            var restaurant = await _backendContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new BadHttpRequestException("No such restaurant");

            _backendContext.Restaurants.Remove(restaurant);
            await _backendContext.SaveChangesAsync();
        }

        public async Task AddStaffToRestaurant(Guid restaurantId, StaffDto data)
        {
            var restaurant = await _backendContext.Restaurants.FirstOrDefaultAsync(x => x.Id == restaurantId)
                ?? throw new BadHttpRequestException("No such restaurant");

            if (await _backendContext.Restaurants.AnyAsync(r => r.Managers.Contains(data.Id) && r.Cooks.Contains(data.Id)))
            {
                throw new BadHttpRequestException("User is already taken");
            }

            if (data.Role == RoleType.Manager)
            {
                restaurant.Managers.Add(data.Id);
            }
            else if (data.Role == RoleType.Cook)
            {
                restaurant.Cooks.Add(data.Id);
            }
            else
            {
                throw new BadHttpRequestException("Unsupported restaurant staff type");
            }

            await _backendContext.SaveChangesAsync();
        }

        public async Task DismissStaffFromRestaurant(Guid restaurantId, Guid staffId, RoleType fromRole)
        {
            var restaurant = await _backendContext.Restaurants.FirstOrDefaultAsync(x => x.Id == restaurantId)
                ?? throw new BadHttpRequestException("No such restaurant");

            if (fromRole == RoleType.Cook)
            {
                restaurant.Cooks.Remove(staffId);
            }

            else if (fromRole == RoleType.Manager)
            {
                restaurant.Managers.Remove(staffId);
            }

            await _backendContext.SaveChangesAsync();
        }

        public async Task UpdateRestaurant(Guid restaurantId, RestaurantUpdateDto data)
        {
            var restaurant = await _backendContext.Restaurants.FirstOrDefaultAsync(x => x.Id == restaurantId)
                ?? throw new BadHttpRequestException("No such restaurant");

            if (await _backendContext.Restaurants.AnyAsync(r => r.Name == data.Name))
            {
                throw new BadHttpRequestException("Name is already taken");
            }

            restaurant.Name = data.Name;

            await _backendContext.SaveChangesAsync();
        }
    }
}
