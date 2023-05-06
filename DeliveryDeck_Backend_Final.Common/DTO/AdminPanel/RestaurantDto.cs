namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PagedUsersDto Staff { get; set; }
    }
}
