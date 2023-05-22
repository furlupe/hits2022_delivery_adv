using AutoMapper;
using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Exceptions;
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

        public async Task BanUser(Guid id)
            => await ChangeUserStatus(id, true);

        public async Task CreateUser(UserCreateDto data)
        {
            var user = new AppUser
            {
                FullName = data.FullName,
                BirthDate = data.BirthDate,
                Gender = data.Gender,
                Email = data.Email
            };

            foreach (var roletype in data.Roles)
            {
                switch (roletype)
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

            foreach (var roletype in data.Roles)
            {
                await _userMgr.AddToRoleAsync(user, roletype.ToString());
            }
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _authContext.Users
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new RepositoryEntityNotFoundException($"No such user w/ id = {id}");

            _authContext.Users.Remove(user);
            await _authContext.SaveChangesAsync();
        }

        public async Task<PagedAvailableStaffDto> GetAvailableStaff(int page = 1)
        {
            var query = await _authContext.Users
               .Include(x => x.Roles)
                   .ThenInclude(r => r.Role)
               .Where(x => !x.IsBanned)
               .ToListAsync();

            var available = new List<AvailableStaffDto>();
            foreach (var user in query)
            {
                var availableAsCook =
                    await _backendContext.Restaurants.AllAsync(r => !r.Cooks.Contains(user.Id))
                    && await _userMgr.IsInRoleAsync(user, RoleType.Cook.ToString());

                var availableAsManager =
                    await _backendContext.Restaurants.AllAsync(r => !r.Managers.Contains(user.Id))
                    && await _userMgr.IsInRoleAsync(user, RoleType.Manager.ToString());

                if (availableAsCook || availableAsManager)
                {
                    available.Add(new AvailableStaffDto
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        AvailableAsCook = availableAsCook,
                        AvailableAsManager = availableAsManager
                    });

                }
            }

            return new PagedAvailableStaffDto
            {
                PageInfo = new PageInfo(available.Count, _UserPageSize, page),
                Users = available.Skip((page - 1) * _UserPageSize).Take(_UserPageSize).ToList()
            };
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
                ?? throw new RepositoryEntityNotFoundException($"No such user w/ id = ${id}");

            if (await _userMgr.IsInRoleAsync(user, RoleType.Admin.ToString()))
            {
                throw new BadHttpRequestException("Denied", StatusCodes.Status403Forbidden);
            }

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
            var query = await _authContext.Users
                .Include(x => x.Roles)
                    .ThenInclude(r => r.Role)
                .Where(x => x.Roles.All(x => x.Role.Type != RoleType.Admin))
                .ToListAsync();

            var response = new PagedUsersDto
            {
                PageInfo = new PageInfo(query.Count, _UserPageSize, page)
            };

            var users = query.Skip((page - 1) * _UserPageSize)
                .Take(_UserPageSize)
                .ToList();

            response.Users = users.Select(_mapper.Map<UserShortDto>).ToList();

            return response;
        }

        public async Task UnbanUser(Guid id)
            => await ChangeUserStatus(id, false);

        public async Task UpdateUser(Guid id, UserUpdateDto data)
        {
            var user = await _authContext.Users
                .Include(x => x.Roles)
                    .ThenInclude(r => r.Role)
                .Include(x => x.Manager)
                .Include(x => x.Cook)
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new RepositoryEntityNotFoundException($"No such user w/ id={id}");

            if (await _userMgr.IsInRoleAsync(user, RoleType.Manager.ToString()) && !data.Roles.Contains(RoleType.Manager))
            {
                var restaurant = await _backendContext.Restaurants.FirstOrDefaultAsync(x => x.Managers.Contains(id));
                restaurant?.Managers.Remove(id);
            }

            if (await _userMgr.IsInRoleAsync(user, RoleType.Cook.ToString()) && !data.Roles.Contains(RoleType.Cook))
            {
                var restaurant = await _backendContext.Restaurants.FirstOrDefaultAsync(x => x.Cooks.Contains(id));
                restaurant?.Cooks.Remove(id);
            }

            user.FullName = data.FullName;
            user.BirthDate = data.BirthDate;
            user.Gender = data.Gender;

            var removedRoles = user.Roles
                .Where(r => !data.Roles.Contains(r.Role.Type))
                .Select(x => x.Role.Type)
                .ToList();

            await _userMgr.RemoveFromRolesAsync(user, removedRoles.Select(x => x.ToString()));

            foreach (var role in removedRoles)
            {
                switch (role)
                {
                    case RoleType.Customer:
                        _authContext.Customers.Remove(user.Customer); break;
                    case RoleType.Manager:
                        _authContext.Managers.Remove(user.Manager); break;
                    case RoleType.Cook:
                        _authContext.Cooks.Remove(user.Cook); break;
                    case RoleType.Courier:
                        _authContext.Couriers.Remove(user.Courier); break;
                }
            }

            foreach (var role in data.Roles.Where(r => !user.Roles.Select(r => r.Role.Type).Contains(r)))
            {
                switch (role)
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

                await _userMgr.AddToRoleAsync(user, role.ToString());
            }

            if (data.Roles.Contains(RoleType.Customer))
            {
                user.Customer.Address = data.Address;
            }

            await _backendContext.SaveChangesAsync();
            await _authContext.SaveChangesAsync();
        }

        private async Task ChangeUserStatus(Guid id, bool isBanned)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new RepositoryEntityNotFoundException($"No such user w/ id = {id}");

            user.IsBanned = isBanned;
            await _authContext.SaveChangesAsync();
        }
    }
}
