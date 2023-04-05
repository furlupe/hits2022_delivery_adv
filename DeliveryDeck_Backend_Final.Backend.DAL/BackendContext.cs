using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Backend.DAL
{
    public class BackendContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cook> Cooks { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishInCart> DishesInCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public BackendContext(DbContextOptions<BackendContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
