using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class ResetPasswordDto
    {
        [Required]
        public string ResetToken { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
