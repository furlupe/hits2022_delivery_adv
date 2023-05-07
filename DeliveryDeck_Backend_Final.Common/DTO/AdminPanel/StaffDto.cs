using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class StaffDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public RoleType Role { get; set; }
    }
}
