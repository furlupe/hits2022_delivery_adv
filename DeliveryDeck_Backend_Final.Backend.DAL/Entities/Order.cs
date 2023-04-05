using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public Cart Cart { get; set; }
        public int Price { get; set; }
        public OrderStatus Status { get; set; }
        public Cook? Cook { get; set; }
        public Courier? Courier { get; set; }
        public Customer Customer { get; set; }
    }
}
