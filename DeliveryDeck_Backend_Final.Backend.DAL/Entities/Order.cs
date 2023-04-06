using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int Price { get; set; }
        public OrderStatus Status { get; set; }
        public Cook? Cook { get; set; }
        public Guid? CourierId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
