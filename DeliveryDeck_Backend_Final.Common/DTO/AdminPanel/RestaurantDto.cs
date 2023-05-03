namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class RestaurantDto
    {
        public string Name { get; set; }
        public List<Guid> Managers { get; set; } = new();
        public List<Guid> Cooks { get; set; } = new();
    }
}
