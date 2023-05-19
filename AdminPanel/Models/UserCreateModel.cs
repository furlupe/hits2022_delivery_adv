using DeliveryDeck_Backend_Final.Common.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Name must not be empty")]
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime birthdate;
        [Required(ErrorMessage = "User must have been born in order to be")]
        public DateTime BirthDate
        {
            get { return birthdate; }
            set
            {
                birthdate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }
        [Required(ErrorMessage = "No sex?")]
        public Gender Gender { get; set; }
        [Required]
        public List<RoleType> Roles { get; set; }
        [Required(ErrorMessage = "E-mail must not be empty")]
        [EmailAddress(ErrorMessage = "Invalid e-mail address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password must not be empty")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@#$%^&-+=()!? _\"]).{8,128}$", ErrorMessage = "Password must contain at least one lower case letter, one upper case letter, one digit & one special symbol")]
        public string Password { get; set; }

        public string? Address { get; set; } = default;
    }
}
