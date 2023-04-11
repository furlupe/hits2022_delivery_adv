using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Services
{
    public class OrderService : IOrderService
    {
        private const int _MinimalCookingTime = 30;
        private readonly BackendContext _backendContext;
        public OrderService(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }
        public async Task CreateOrder(Guid userId, CreateOrderDto data)
        {
            var cart = await _backendContext.Carts
                .Include(c => c.Dishes)
                    .ThenInclude(d => d.Dish)
                .SingleAsync(c => c.CustomerId == userId);

            var menus = await _backendContext.Menus
                .Include(m => m.Dishes)
                .Where(m => m.Dishes
                    .Any(dish => cart.Dishes
                        .Select(d => d.Dish)
                        .Contains(dish)
                        )
                    )
                .Distinct()
                .ToListAsync();

            var orders = new List<Order>();
            var restaurants = menus.Select(m => m.Restaurant).Distinct();
            foreach(var restaurant in restaurants)
            {
                var orderPrice = 0;
                var menuDishes = cart.Dishes
                    .Where(d => menus
                        .Any(m => m.Restaurant == restaurant 
                        && m.Dishes.Contains(d.Dish))
                    )
                    .ToList();

                foreach (var dishInCart in menuDishes)
                {
                    orderPrice += dishInCart.Amount * dishInCart.Dish.Price;
                }

                orders.Add(new Order
                {
                    OrderTime = DateTime.UtcNow,
                    DeliveryTime = DateTime.UtcNow.AddMinutes(_MinimalCookingTime + new Random().Next(10, 100)),
                    Price = orderPrice,
                    Status = OrderStatus.Created,
                    CustomerId = userId,
                    Dishes = menuDishes
                });
            }

            await _backendContext.Orders.AddRangeAsync(orders);

            cart.WasOrdered = true;

            await _backendContext.Carts.AddAsync(new Cart { CustomerId = userId });
            await _backendContext.SaveChangesAsync();
        }
    }
}
