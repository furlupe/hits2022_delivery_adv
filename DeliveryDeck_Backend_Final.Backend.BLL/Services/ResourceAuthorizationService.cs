using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Services
{
    public class ResourceAuthorizationService : IResourceAuthorizationService
    {
        private readonly BackendContext _backendContext;

        public ResourceAuthorizationService(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }

        public async Task<bool> DishInCartResourceExists(Guid userId, Guid dishId) 
            => await _backendContext.Carts
            .FirstOrDefaultAsync(
                c => c.CustomerId == userId 
                && c.Dishes
                    .Select(d => d.Dish.Id)
                    .Contains(dishId)
                ) is not null;

        public Task<bool> DishResourceExists(Guid dishId) 
            => ResourceIsAvailable<Dish>(dishId);

        public async Task<bool> OrderResourceExists(Guid userId, int orderId)
            => await _backendContext.Orders
            .FirstOrDefaultAsync(
                c => c.CustomerId == userId
                && c.Id == orderId
                ) is not null;

        public async Task<bool> MenuResourceExists(Guid menuId)
            => await ResourceIsAvailable<Menu>(menuId);

        public async Task<bool> RestaurantResourceExists(Guid restaurantId)
            => await _backendContext.Restaurants
            .FirstOrDefaultAsync(r => r.Id == restaurantId) is not null;

        public async Task<bool> RestaurantMenuResourceExists(Guid manager, Guid menuId)
            => await _backendContext.Menus
                .FirstOrDefaultAsync(m => m.Id == menuId && m.Restaurant.Managers.Contains(manager)) is not null;

        public async Task<bool> RestaurantDishResourceExists(Guid manager, Guid dishId)
            => await _backendContext.Restaurants
                .FirstOrDefaultAsync(
                    r => r.Managers.Contains(manager) 
                    && r.Dishes.Any(d => d.Id == dishId)
                ) is not null;

        private async Task<bool> ResourceIsAvailable<TEntity>(object key)
            where TEntity : class
            => await _backendContext.FindAsync<TEntity>(key) is not null;

    }
}
