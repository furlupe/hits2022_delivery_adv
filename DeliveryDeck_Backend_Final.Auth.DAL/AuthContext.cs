using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Auth.DAL
{
    public class AuthContext : IdentityDbContext<AppUser, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, RoleClaim, IdentityUserToken<Guid>>
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
        public override DbSet<AppUser> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public override DbSet<UserRole> UserRoles { get; set; }
        public override DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<RefreshUserToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(o =>
            {
                o.HasMany(o => o.Roles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Role>(o =>
            {
                o.HasMany(r => r.Users)
                    .WithOne(r => r.Role)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();

                o.HasMany(r => r.RoleClaims)
                    .WithOne(c => c.Role)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();
            });
        }
    }
}
