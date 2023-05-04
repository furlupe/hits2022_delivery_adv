using AutoMapper;
using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.BLL.Services
{
    public class AdminUserService : IAdminUserService
    {
        private const int _UserPageSize = 2;
        private readonly AuthContext _authContext;
        private readonly IMapper _mapper;

        public AdminUserService(AuthContext authContext, IMapper mapper)
        {
            _authContext = authContext;
            _mapper = mapper;
        }

        public async Task<PagedUsersDto> GetUsers(int page = 1)
        {

            var response = new PagedUsersDto
            {
                PageInfo = new PageInfo(_authContext.Users.Count(), _UserPageSize, page)
            };

            var users = await _authContext.Users
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
