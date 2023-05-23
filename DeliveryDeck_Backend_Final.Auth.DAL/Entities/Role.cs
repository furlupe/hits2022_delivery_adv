using DeliveryDeck_Backend_Final.Common.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace DeliveryDeck_Backend_Final.Auth.DAL.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public RoleType Type { get; set; }
        public ICollection<UserRole> Users { get; set; }
    }
}
