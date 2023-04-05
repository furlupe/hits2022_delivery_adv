namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
    }
}
