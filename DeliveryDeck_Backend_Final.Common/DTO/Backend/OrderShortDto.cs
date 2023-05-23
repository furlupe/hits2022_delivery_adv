using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class OrderShortDto
    {
        public int Number { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int Price { get; set; }
        public OrderStatus Status { get; set; }
        public string Address { get; set; }
    }
}
