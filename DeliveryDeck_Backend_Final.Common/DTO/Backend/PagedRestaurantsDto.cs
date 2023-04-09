namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class PagedRestaurantsDto
    {
        public ICollection<RestaurantShortDto> Restaurants { get; set; } = new List<RestaurantShortDto>();
        public PageInfo PageInfo { get; set; }
    }
}
