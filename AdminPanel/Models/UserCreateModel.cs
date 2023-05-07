using DeliveryDeck_Backend_Final.Common.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class UserCreateModel
    {
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime birthdate;
        public DateTime BirthDate {
            get { return birthdate; }
            set 
            {
                birthdate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public List<RoleType> Roles { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string? Address { get; set; } = default;
    }
}
