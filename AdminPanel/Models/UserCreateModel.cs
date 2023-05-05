using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace AdminPanel.Models
{
    public class UserCreateModel
    {
        public string FullName { get; set; }
        public DateTime birthdate;
        public DateTime BirthDate {
            get { return birthdate; }
            set 
            {
                birthdate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }
        public Gender Gender { get; set; }
        public List<RoleType> Roles { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string? Address { get; set; } = default;
    }
}
