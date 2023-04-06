namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public ICollection<DishInCart> Dishes { get; set; } = new List<DishInCart>();
        public Guid CustomerId { get; set; }
        public Order? Order { get; set; } = null;
    }
}
