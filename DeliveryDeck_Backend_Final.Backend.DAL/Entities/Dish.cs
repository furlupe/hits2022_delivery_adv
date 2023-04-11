using DeliveryDeck_Backend_Final.Common.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public bool IsVegeterian { get; set; }
        public string? Photo { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public FoodCategory Category { get; set; }

        [NotMapped]
        public double Rating
        {
            get
            {
                return (Ratings.Count > 0) ? Ratings.Average(r => r.Value) : 0;
            }
        }
    }
}
