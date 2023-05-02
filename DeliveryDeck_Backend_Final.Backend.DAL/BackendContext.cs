using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Backend.DAL
{
    public class BackendContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishInCart> DishesInCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public BackendContext(DbContextOptions<BackendContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Menu>()
                .HasMany(m => m.Dishes)
                .WithMany();

            builder.Entity<Order>()
                .HasKey(m => m.Id);

            builder.Entity<Order>()
                .HasOne(o => o.Restaurant)
                .WithMany(r => r.Orders);

            builder.Entity<Order>()
                .HasMany(o => o.Dishes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Restaurant>()
                .HasMany(r => r.Dishes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Restaurant>()
                .HasData(
                    new Restaurant { Id = Guid.NewGuid(), Name = "New Amogus" },
                    new Restaurant { Id = Guid.NewGuid(), Name = "Old Amogus" },
                    new Restaurant { Id = Guid.NewGuid(), Name = "FeastingHub" }
                    );

        }
    }
}
