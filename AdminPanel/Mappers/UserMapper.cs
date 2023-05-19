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

            CreateMap<UserExtendedDto, UserModel>();

            CreateMap<AvailableStaffDto, AvailableStaffModel>();
            CreateMap<PagedAvailableStaffDto, AvailableStaffListModel>();

            CreateMap<UserCreateModel, UserCreateDto>();
            CreateMap<UserUpdateModel, UserUpdateDto>();
        }
    }
}
