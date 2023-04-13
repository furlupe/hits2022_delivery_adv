namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class MenuInfo
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public PagedDishesDto Dishes { get; set; } = new ();
    }
}
