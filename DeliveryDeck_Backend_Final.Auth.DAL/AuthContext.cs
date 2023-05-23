using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Auth.DAL
{
    public class AuthContext : IdentityDbContext<AppUser, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
        public DbSet<RefreshUserToken> RefreshTokens { get; set; }
        public DbSet<Cook> Cooks { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(o =>
            {
                o.HasMany(o => o.Roles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                o.HasOne(x => x.Cook)
                    .WithOne(c => c.User)
                    .HasForeignKey<Cook>()
                    .OnDelete(DeleteBehavior.Cascade);

                o.HasOne(x => x.Manager)
                    .WithOne(c => c.User)
                    .HasForeignKey<Manager>()
                    .OnDelete(DeleteBehavior.Cascade);

                o.HasOne(x => x.Courier)
                    .WithOne(c => c.User)
                    .HasForeignKey<Courier>()
                    .OnDelete(DeleteBehavior.Cascade);

                o.HasOne(x => x.Customer)
                    .WithOne(c => c.User)
                    .HasForeignKey<Customer>()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Role>(o =>
            {
                o.HasMany(r => r.Users)
                    .WithOne(r => r.Role)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();
            });
        }
    }
}
