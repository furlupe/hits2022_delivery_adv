using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Auth
{
    public class ResetPasswordShortDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
