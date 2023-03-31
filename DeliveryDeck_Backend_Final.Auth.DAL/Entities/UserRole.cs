using Microsoft.AspNetCore.Identity;

namespace DeliveryDeck_Backend_Final.Auth.DAL.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual AppUser User { get; set; }
        public virtual Role Role { get; set; }

    }
}
