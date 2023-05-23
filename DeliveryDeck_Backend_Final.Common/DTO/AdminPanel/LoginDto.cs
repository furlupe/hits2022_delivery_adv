using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
