using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Rating
    {
        [Key]
        public Guid Id { get; set; }
        public Dish Dish { get; set; }
        public Guid AuthorId { get; set; }
        public int Value { get; set; }
    }
}
