using AutoMapper;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;

namespace AdminPanel.BLL.Mappers
{
    public class DtoToEntityMapper : Profile
    {
        public DtoToEntityMapper() 
        {
            CreateMap<RestaurantDto, Restaurant>()
                .ReverseMap();
        }
    }
}
