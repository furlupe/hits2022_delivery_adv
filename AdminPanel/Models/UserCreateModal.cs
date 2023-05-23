using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace AdminPanel.Models
{
    public class UserCreateModal
    {
        public string Action { get; set; } = "Create";

        public bool NameIsReadonly { get; set; } = false;
        public string Name { get; set; } = string.Empty;

        public bool EmailIsReadonly { get; set; } = false;
        public string Email { get; set; } = string.Empty;

        public bool PasswordIsAccessible { get; set; } = true;

        public bool BirthdateIsReadonly { get; set; } = false;
        public DateTime Birthdate { get; set; } = default;

        public bool SexIsReadonly { get; set; } = false;
        public Gender Sex { get; set; } = default;

        public bool RolesIsReadonly { get; set; } = false;
        public List<RoleType> Roles { get; set; } = new();

        public bool AddressIsReadonly { get; set; } = false;
        public string Address { get; set; } = string.Empty;
    }
}
