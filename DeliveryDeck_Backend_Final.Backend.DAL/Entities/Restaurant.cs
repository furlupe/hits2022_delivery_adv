namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Restaurant : NamedEntity
    {
        public Guid Id { get; set; }
        public List<Guid> Managers { get; set; } = new();
        public List<Guid> Cooks { get; set; } = new();
        public List<Menu> Menus { get; set; }
        public List<Dish> Dishes { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}
