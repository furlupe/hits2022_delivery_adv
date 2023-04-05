namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Courier
    {
        public Guid Id { get; set; }
        public ICollection<Order> Orders;
    }
}
