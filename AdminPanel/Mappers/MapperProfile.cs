using AdminPanel.Models;
using AutoMapper;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace AdminPanel.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<RestaurantCreateModel, RestaurantShortDto>();

            CreateMap<RestaurantShortModel, RestaurantShortDto>()
                .ReverseMap();

            CreateMap<RestaurantDto, RestaurantModel>();

            CreateMap<RestaurantListModel, PagedRestaurantsDto>()
                .ReverseMap();
        }
    }
}
