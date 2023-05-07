using AdminPanel.Models;
using AutoMapper;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace AdminPanel.Mappers
{
    public class RestaurantMapper : Profile
    {
        public RestaurantMapper() 
        {
            CreateMap<RestaurantShortModel, RestaurantShortDto>()
                .ReverseMap();

            CreateMap<RestaurantDto, RestaurantModel>();

            CreateMap<RestaurantListModel, PagedRestaurantsDto>()
                .ReverseMap();

            CreateMap<StaffModel, StaffDto>()
                .ReverseMap();

            CreateMap<PagedStaffDto, StaffListModel>()
                .ForMember(dest => dest.Staff, ost => ost.MapFrom(src => src.Staff));

            CreateMap<RestaurantCreateModel, RestaurantCreateDto>();
        }
    }
}
