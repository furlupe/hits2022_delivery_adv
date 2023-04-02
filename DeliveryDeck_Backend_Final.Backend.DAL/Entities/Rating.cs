using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Rating
    {
        [Key]
        public Dish Dish { get; set; }
        public Customer Author { get; set; }
        public int Value { get; set; }
    }
}
