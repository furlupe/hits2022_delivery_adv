namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Menu : NamedEntity
    {
        public Guid Id { get; set; }
        public List<Dish> Dishes { get; set; }
        public Restaurant Restaurant { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
