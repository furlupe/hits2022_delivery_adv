using AdminPanel.Models;
using AutoMapper;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;

namespace AdminPanel.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() 
        {
            CreateMap<PagedUsersDto, UserListModel>();
            CreateMap<UserShortDto, UserShortModel>();
        }
    }
}
