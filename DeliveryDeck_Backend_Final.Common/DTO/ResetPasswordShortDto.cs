using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class ResetPasswordShortDto
    {
        [Required]
        public string Email { get; set; }
    }
}
