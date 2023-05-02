using AdminPanel.Models;
using AutoMapper;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;

namespace AdminPanel.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<RestaurantModel, RestaurantDto>();
        }
    }
}
