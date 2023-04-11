using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; } = 0;
        public DateTime OrderTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int Price { get; set; }
        public OrderStatus Status { get; set; }
        public string Address { get; set; }
        public Guid? Cook { get; set; }
        public Guid? CourierId { get; set; }
        public Guid CustomerId { get; set; }
        public ICollection<DishInCart> Dishes { get; set; }
    }
}
