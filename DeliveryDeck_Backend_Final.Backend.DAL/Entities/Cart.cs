namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public ICollection<DishInCart> Dishes { get; set; } = new List<DishInCart>();
        public Guid CustomerId { get; set; }
        public bool WasOrdered { get; set; } = false;
    }
}
