using DeliveryDeck_Backend_Final.Common.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class UserUpdateDto
    {
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public List<RoleType> Roles { get; set; }
        public string? Address { get; set; } = default;
    }
}
