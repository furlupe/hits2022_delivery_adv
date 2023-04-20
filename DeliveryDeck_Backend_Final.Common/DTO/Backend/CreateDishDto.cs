using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Utils;
using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class CreateDishDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsVegetarian { get; set; }
        [Required]
        public string? Photo { get; set; }
        [Required]
        [EnumValueDefined]
        public FoodCategory FoodCategory { get; set; }
    }
}
