using DeliveryDeck_Backend_Final.Common.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class UserUpdateModel 
    {
        public Guid Id { get; set; }

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
        [Required(ErrorMessage = "User must have at least one role")]
        public List<RoleType> Roles { get; set; }
        public string? Address { get; set; } = default;

    }

}
