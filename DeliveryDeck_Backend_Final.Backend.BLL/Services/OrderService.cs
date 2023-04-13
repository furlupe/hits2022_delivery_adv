using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task CancelOrder(Guid userId, int orderId)
        {
            var order = await GetUserOrder(userId, orderId);

            if (order.Status != OrderStatus.Created)
            {
                throw new BadHttpRequestException("Too late, m8, the order has been proceeded");
            }

            order.Status = OrderStatus.Cancelled;
            await _backendContext.SaveChangesAsync();
        }

        public async Task ChangeOrderStatus(Guid userId, int orderId, OrderStatus status)
        {
            var order = await GetUserOrder(userId, orderId);

            if (status - order.Status > 1 && status != OrderStatus.Cancelled)
            {
                throw new BadHttpRequestException("Huge status step > 1");
            }

            if (userId == order.Cook && (status == OrderStatus.Created || status > OrderStatus.ReadyForDelivery)
               || userId == order.CourierId && status < OrderStatus.Delivering
               || userId == order.CustomerId || (userId != order.CourierId && userId != order.Cook))
            {
                throw new BadHttpRequestException("Can't change the status");
            }

            order.Status = status;
            await _backendContext.SaveChangesAsync();
        }

        public async Task<List<Guid>> CreateOrder(Guid userId, CreateOrderDto data)
        {
            var cart = await _backendContext.Carts
                .Include(c => c.Dishes)
                    .ThenInclude(d => d.Dish)
                .FirstAsync(c => c.CustomerId == userId);

            // находим все уникальные меню, в которых лежат те или иные блюда
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

            var restaurants = menus
                .Select(m => m.Restaurant)
                .Distinct();

            var orders = new List<Order>();
            var removed = new List<Guid>();

            foreach(var restaurant in restaurants)
            {
                var totalPrice = 0;

                // блюда из активных меню
                var menuDishesFromCart = cart.Dishes
                    .IntersectBy(
                        restaurant.Menus
                            .Where(m => m.IsActive == true)
                            .SelectMany(m => m.Dishes), 
                        x => x.Dish);

                // блюда из неактивных меню
                var removedDishes = cart.Dishes
                    .IntersectBy(
                        restaurant.Menus
                            .Where(m => m.IsActive == false)
                            .SelectMany(m => m.Dishes),
                        x => x.Dish);

                if (!removedDishes.IsNullOrEmpty())
                {
                    removed.AddRange(removedDishes.Select(x => x.Dish.Id));
                }

                var orderedDishes = new List<DishInCart>();
                foreach (var dishInCart in menuDishesFromCart)
                {
                    totalPrice += dishInCart.Amount * dishInCart.Dish.Price;
                    dishInCart.PriceWhenOrdered = dishInCart.Dish.Price;
                }

                orders.Add(new Order
                {
                    OrderTime = DateTime.UtcNow,
                    DeliveryTime = DateTime.UtcNow.AddMinutes(_MinimalCookingTime + new Random().Next(10, 100)),
                    Price = totalPrice,
                    Status = OrderStatus.Created,
                    CustomerId = userId,
                    Dishes = menuDishesFromCart.ToList(),
                    Address = data.Address
                });
            }

            await _backendContext.Orders.AddRangeAsync(orders);

            cart.WasOrdered = true;

            await _backendContext.Carts.AddAsync(new Cart { CustomerId = userId });
            await _backendContext.SaveChangesAsync();

            return removed;
        }

        public async Task<OrderPagedDto> GetHistory(
            Guid userId,
            int? number = default,
            DateTime fromDate = default,
            int page = 1,
            bool activeOnly = false
            )
        {
            var query = _backendContext.Orders
                .Where(o => o.CustomerId == userId && o.OrderTime >= fromDate);

            var orders = new List<Order>();
            var response = new OrderPagedDto();

            if (number == null && activeOnly)
            {
                query = query
                    .Where(o =>
                        o.Status != OrderStatus.Cancelled
                        && o.Status != OrderStatus.Delivered
                    );
            }
            else if (number != null)
            {
                query = query.Where(o => o.Id == number);
            }

            orders = await query.ToListAsync();
            response = new OrderPagedDto { PageInfo = new PageInfo(orders.Count, _OrdersPageSize, page) };

            foreach (var order in orders)
            {
                response.Orders.Add(new OrderShortDto
                {
                    Number = order.Id,
                    DeliveryTime = order.DeliveryTime,
                    OrderTime = order.OrderTime,
                    Price = order.Price,
                    Status = order.Status,
                    Address = order.Address
                });
            }

            return response;
        }

        public async Task<OrderDto> GetOrderDetails(Guid userId, int orderId)
        {
            var order = await GetUserOrder(userId, orderId);
            var dishes = new List<DishCartDto>();

            foreach (var dishInCart in order.Dishes)
            {
                var dish = dishInCart.Dish;
                dishes.Add(new DishCartDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dishInCart.PriceWhenOrdered,
                    IsVegeterian = dish.IsVegeterian,
                    Photo = dish.Photo,
                    Amount = dishInCart.Amount
                });
            }

            return new OrderDto
            {
                Number = order.Id,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Status = order.Status,
                Address = order.Address,
                DeliveryTime = order.DeliveryTime,
                Dishes = dishes
            };
        }

        private async Task<Order> GetUserOrder(Guid userId, int orderId)
        {
            var order = await _backendContext.Orders
                .Include(o => o.Dishes)
                    .ThenInclude(dc => dc.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId)
                ?? throw new BadHttpRequestException("No such order, moron");

            if (order.CustomerId != userId)
            {
                throw new BadHttpRequestException("Access denied", StatusCodes.Status403Forbidden);
            }

            return order;
        }
    }
}
