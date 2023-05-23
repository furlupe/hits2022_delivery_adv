using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class RestaurantCreateDto
    {
        [Required(ErrorMessage = "Name field must not be empty")]
        [MinLength(1)]
        public string Name { get; set; }
    }
}
