using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class UserCreateDto
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public List<RoleType> Roles { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string? Address { get; set; } = default;
    }
}
