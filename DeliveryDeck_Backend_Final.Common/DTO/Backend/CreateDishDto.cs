using DeliveryDeck_Backend_Final.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public FoodCategory FoodCategory { get; set; }
    }
}
