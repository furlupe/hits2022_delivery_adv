using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace AdminPanel.Models
{
    public class UserShortModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RoleType> Roles { get; set; }
    }
}
