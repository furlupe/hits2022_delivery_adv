using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class DishInCart
    {
        [Key]
        public Guid Id { get; set; }
        public Dish Dish { get; set; }
        public Cart Cart { get; set; }
        public int Amount { get; set; }

        public int PriceWhenOrdered { get; set; } = 0;

    }
}
