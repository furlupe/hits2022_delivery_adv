using AutoMapper;
using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.BLL.Services
{
    public class AdminUserService : IAdminUserService
    {
        private const int _UserPageSize = 2;
        private readonly AuthContext _authContext;
        private readonly UserManager<AppUser> _userMgr;
        private readonly BackendContext _backendContext;
        private readonly IMapper _mapper;

        public AdminUserService(AuthContext authContext, BackendContext backendContext, UserManager<AppUser> userMgr, IMapper mapper)
        {
            _authContext = authContext;
            _mapper = mapper;
            _backendContext = backendContext;
            _userMgr = userMgr;
        }

        public async Task CreateUser(UserCreateDto data)
        {
            var user = new AppUser
            {
                FullName = data.FullName,
                BirthDate = data.BirthDate,
                Gender = data.Gender,
                Email = data.Email
            };

            if(data.Roles.Contains(RoleType.Customer))
            {
                user.Customer = new Customer { Address = data.Address };
            }

            foreach(var roletype in data.Roles)
            {
                switch(roletype)
                {
                    case RoleType.Customer:
                        user.Customer = new Customer { Address = data.Address }; break;
                    case RoleType.Manager:
                        user.Manager = new Manager(); break;
                    case RoleType.Cook:
                        user.Cook = new Cook(); break;
                    case RoleType.Courier:
                        user.Courier = new Courier(); break;
                }
            }

            await _userMgr.CreateAsync(user, data.Password);

            foreach(var roletype in data.Roles)
            {
                await _userMgr.AddToRoleAsync(user, roletype.ToString());
            }
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _authContext.Users
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new BadHttpRequestException("No such user");

            _authContext.Users.Remove(user);
            await _authContext.SaveChangesAsync();
        }

        public async Task<UserExtendedDto> GetUserInfo(Guid id)
        {
            var user = await _authContext.Users
                .Include(x => x.Roles)
                    .ThenInclude(r => r.Role)
                .Include(x => x.Customer)
                .Include(x => x.Manager)
                .Include(x => x.Cook)
                .Include(x => x.Courier)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new BadHttpRequestException("No such user");

            var response = _mapper.Map<UserExtendedDto>(user);

            if (user.Customer is not null)
            {
                response.Address = user.Customer.Address;
            }

            response.RestaurantAsManager = 
                user.Manager is not null ? _mapper.Map<RestaurantShortDto>(
                    await _backendContext.Restaurants.FirstOrDefaultAsync(r => r.Managers.Contains(user.Manager.Id))
                    ) : null;

            response.RestaurantAsCook =
                user.Cook is not null ? _mapper.Map<RestaurantShortDto>(
                    await _backendContext.Restaurants.FirstOrDefaultAsync(r => r.Cooks.Contains(user.Cook.Id))
                    ) : null;

            return response;
        }

        public async Task<PagedUsersDto> GetUsers(int page = 1)
        {

            var response = new PagedUsersDto
            {
                PageInfo = new PageInfo(_authContext.Users.Count(), _UserPageSize, page)
            };

            var users = await _authContext.Users
                .Include(x => x.Roles)
                    .ThenInclude(r => r.Role)
                .Skip((page - 1) * _UserPageSize)
                .Take(_UserPageSize)
                .ToListAsync();

            foreach (var user in users)
            {
                response.Users.Add(_mapper.Map<UserShortDto>(user));
            }

            return response;
        }
    }
}
