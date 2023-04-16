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
                .AnyAsync(
                    c => c.CustomerId == userId 
                    && c.Dishes.Select(d => d.Dish.Id).Contains(dishId)
                );

        public Task<bool> DishResourceExists(Guid dishId) 
            => ResourceIsAvailable<Dish>(dishId);

        public async Task<bool> OrderCustomerRelationExists(Guid userId, int orderId)
            => await _backendContext.Orders
                .AnyAsync(
                    c => c.CustomerId == userId
                    && c.Id == orderId
                );

        public async Task<bool> MenuResourceExists(Guid menuId)
            => await ResourceIsAvailable<Menu>(menuId);

        public async Task<bool> RestaurantResourceExists(Guid restaurantId)
            => await _backendContext.Restaurants
            .FirstOrDefaultAsync(r => r.Id == restaurantId) is not null;

        public async Task<bool> ManagerRestaurantMenuResourceExists(Guid manager, Guid menuId)
            => await _backendContext.Menus
                .AnyAsync(
                    m => m.Id == menuId 
                    && m.Restaurant.Managers.Contains(manager)
                );

        public async Task<bool> ManagerRestaurantDishResourceExists(Guid manager, Guid dishId)
            => await _backendContext.Restaurants
                .AnyAsync(
                    r => r.Managers.Contains(manager) 
                    && r.Dishes.Any(d => d.Id == dishId)
                );

        public async Task<bool> StaffRestaurantOrderResourceExists(Guid userId, int orderId)
            => await _backendContext.Restaurants
                .AnyAsync(
                    r => (r.Cooks.Contains(userId) || r.Managers.Contains(userId)) 
                    && r.Orders.Any(o => o.Id == orderId)
                );

        public async Task<bool> OrderCookRelationExists(Guid cookId, int orderId)
            => await _backendContext.Orders
                .AnyAsync(
                    o => o.Cook == cookId 
                    && o.Id == orderId
                );

        public async Task<bool> OrderIsAccessibleForCourier(Guid courierId, int orderId)
            => await _backendContext.Orders
                .AnyAsync(
                    x => (x.CourierId == courierId || x.CourierId == null)
                    && x.Id == orderId
                );
        public async Task<bool> OrderCourierRelationExists(Guid courierId, int orderId)
            => await _backendContext.Orders
                .AnyAsync(
                    x => x.CourierId == courierId
                    && x.Id == orderId
                );

        private async Task<bool> ResourceIsAvailable<TEntity>(object key)
            where TEntity : class
            => await _backendContext.FindAsync<TEntity>(key) is not null;
    }
}
