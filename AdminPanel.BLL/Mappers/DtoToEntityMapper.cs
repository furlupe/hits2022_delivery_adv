using AutoMapper;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace AdminPanel.BLL.Mappers
{
    public class DtoToEntityMapper : Profile
    {
        public DtoToEntityMapper() 
        {
            CreateMap<RestaurantDto, Restaurant>()
                .ReverseMap();

            CreateMap<AppUser, StaffDto>();
            CreateMap<AppUser, AvailableStaffDto>();

            CreateMap<Restaurant, RestaurantShortDto>();

            CreateMap<AppUser, UserShortDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RoleTypes));

            CreateMap<AppUser, UserExtendedDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RoleTypes));
        }
    }
}
