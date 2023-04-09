namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> Managers { get; set; } = new();
        public List<Guid> Cooks { get; set; } = new();
        public ICollection<Menu> Menus { get; set; }
    }
}
