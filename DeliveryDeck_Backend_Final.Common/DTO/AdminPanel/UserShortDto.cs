using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class UserShortDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public bool IsBanned { get; set; }
        public List<RoleType> Roles { get; set; } = new();
    }
}
