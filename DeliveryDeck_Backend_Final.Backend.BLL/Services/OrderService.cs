using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Services
{
    public class OrderService : IOrderService
    {
        private const int _MinimalCookingTime = 30;
        private const int _OrdersPageSize = 1;
        private readonly BackendContext _backendContext;
        public OrderService(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }

        public async Task CancelOrder(Guid userId, Guid orderId)
        {
            var order = await _backendContext.Orders
                .SingleOrDefaultAsync(o => o.Id == orderId)
                ?? throw new BadHttpRequestException("No such order, moron");

            if (order.CustomerId != userId)
            {
                throw new BadHttpRequestException("Access denied", StatusCodes.Status403Forbidden);
            }

            if (order.Status != OrderStatus.Created)
            {
                throw new BadHttpRequestException("Too late, m8, the order is in the kitchen");
            }

            order.Status = OrderStatus.Cancelled;
            await _backendContext.SaveChangesAsync();
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
                    Dishes = menuDishes,
                    Address = data.Address,

                    // можно сделать через .Aggregate((x, y) => x.Number > y.Number ? x : y), но вызов не будет асинхронным
                    Number = (await _backendContext.Orders.Select(o => o.Number).Where(_ => true).ToListAsync()).Max() + 1
                });
            }

            await _backendContext.Orders.AddRangeAsync(orders);

            cart.WasOrdered = true;

            await _backendContext.Carts.AddAsync(new Cart { CustomerId = userId });
            await _backendContext.SaveChangesAsync();
        }

        public async Task<OrderPagedDto> GetHistory(Guid userId, int page = 1, bool activeOnly = false)
        {
            var query = _backendContext.Orders
                .Where(o => o.CustomerId == userId);
            
            if(activeOnly)
            {
                query = query
                    .Where(o => 
                        o.Status != OrderStatus.Cancelled
                        && o.Status != OrderStatus.Delivered
                    );
            }

            var orders = await query.ToListAsync();

            var response = new OrderPagedDto { PageInfo = new PageInfo(orders.Count, _OrdersPageSize, page) };

            foreach (var order in orders)
            {
                response.Orders.Add(new OrderShortDto
                {
                    Id = order.Id,
                    DeliveryTime = order.DeliveryTime,
                    OrderTime = order.OrderTime,
                    Price = order.Price,
                    Status = order.Status,
                    Address = order.Address
                });
            }

            return response;
        }
    }
}
