using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class CreateOrderDto
    {
        [Required]
        public string Address { get; set; }
    }
}
