using Microsoft.AspNetCore.Identity;

namespace DeliveryDeck_Backend_Final.Auth.DAL.Entities
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
    }
}
