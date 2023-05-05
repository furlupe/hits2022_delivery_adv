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
            CreateMap<UserShortDto, UserShortModel>()
                .ForMember(dest => dest.FullName, ost => ost.MapFrom(src => src.Name));
            CreateMap<UserExtendedDto, UserModel>();

            CreateMap<UserCreateModel, UserCreateDto>();
        }
    }
}
