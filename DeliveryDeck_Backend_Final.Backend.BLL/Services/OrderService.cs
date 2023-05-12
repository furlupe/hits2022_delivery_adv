using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Services
{
    public class OrderService : IOrderService
    {
        private const int _MinimalCookingTime = 30;
        private const int _OrdersPageSize = 1;
        private readonly BackendContext _backendContext;
        private readonly IRabbitMqService _rabbitService;
        public OrderService(BackendContext backendContext, IRabbitMqService rabbitService)
        {
            _backendContext = backendContext;
            _rabbitService = rabbitService;
        }

        public async Task CancelOrder(int orderId)
        {
            var order = await GetOrder(orderId);

            if (order.Status != OrderStatus.Created)
            {
                throw new BadHttpRequestException($"Can't change status from {order.Status} to Cancelled");
            }

            order.Status = OrderStatus.Cancelled;
            await _backendContext.SaveChangesAsync();
        }
        public async Task<RemovedDishesDto> CreateOrder(Guid userId, CreateOrderDto data)
        {
            var cart = await _backendContext.Carts
                .Include(c => c.Dishes)
                    .ThenInclude(d => d.Dish)
                .FirstAsync(c => c.CustomerId == userId && c.WasOrdered == false);

            if (cart.Dishes.IsNullOrEmpty())
            {
                throw new BadHttpRequestException("Your cart is empty");
            }

            var menus = await _backendContext.Menus
                .Include(m => m.Dishes)
                .Include(m => m.Restaurant)
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

            foreach (var restaurant in restaurants)
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
                    Address = data.Address,
                    Restaurant = restaurant
                });
            }

            await _backendContext.Orders.AddRangeAsync(orders);

            cart.WasOrdered = true;

            await _backendContext.Carts.AddAsync(new Cart { CustomerId = userId });
            await _backendContext.SaveChangesAsync();

            return new RemovedDishesDto { RemovedDishes = removed };
        }

        public async Task<OrderAvailablePagedDto> GetAvailableForKitchen(Guid userId, OrderSortingType? sortBy, int page = 1)
        {
            var orders = await _backendContext.Restaurants
                .Where(r => r.Cooks.Contains(userId))
                .Select(r => r.Orders)
                .FirstAsync();

            var sortedOrders = orders.Where(o => o.Status == OrderStatus.Created);
            sortedOrders = sortedOrders.SortByMethod(sortBy);

            return CreateOrderAvailablePagedResponse(sortedOrders, page);
        }

        public async Task<OrderDto> GetOrderDetails(int orderId)
        {
            var order = await GetOrder(orderId);
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

        public async Task<RemovedDishesDto> RepeatPreviousOrder(int orderId)
        {
            var order = await GetOrder(orderId);
            var restaurantDishes = await _backendContext.Restaurants
                .Where(r => r.Id == order.Restaurant.Id)
                .Select(r => r.Dishes)
                .FirstOrDefaultAsync()
                ?? throw new BadHttpRequestException("The restaurant does not exist");

            var dishes = order.Dishes
                .IntersectBy(restaurantDishes, x => x.Dish);

            var removedDishes = order.Dishes
                .ExceptBy(restaurantDishes, x => x.Dish);

            var totalPrice = 0;

            var addedDishes = new List<DishInCart>();
            foreach (var dishInCart in dishes)
            {
                totalPrice += dishInCart.Dish.Price * dishInCart.Amount;
                addedDishes.Add(new DishInCart
                {
                    Dish = dishInCart.Dish,
                    Amount = dishInCart.Amount,
                    PriceWhenOrdered = dishInCart.Dish.Price
                });
            }

            await _backendContext.Carts.AddAsync(new Cart
            {
                CustomerId = order.CustomerId,
                WasOrdered = true,
                Dishes = addedDishes
            });

            await _backendContext.Orders.AddAsync(new Order
            {
                OrderTime = DateTime.UtcNow,
                DeliveryTime = DateTime.UtcNow.AddMinutes(_MinimalCookingTime + new Random().Next(10, 100)),
                Price = totalPrice,
                Status = OrderStatus.Created,
                Address = order.Address,
                CustomerId = order.CustomerId,
                Dishes = addedDishes,
                Restaurant = order.Restaurant
            });

            await _backendContext.SaveChangesAsync();

            return new RemovedDishesDto { RemovedDishes = removedDishes.Select(d => d.Dish.Id) };

        }

        public async Task SetOrderToDeliveryAvailable(int orderId)
        {
            var order = await _backendContext.Orders
                .FirstAsync(x => x.Id == orderId);

            if (order.Status > OrderStatus.Packaging)
            {
                throw new BadHttpRequestException("Order is already waiting for being delivered");
            }

            order.Status = OrderStatus.ReadyForDelivery;
            await _backendContext.SaveChangesAsync();

            _rabbitService.SendMessage(order.CustomerId.ToString(), $"Order [No. {order.Id}] now has status [{order.Status}]");

        }

        public async Task TakeOrderToKitchen(Guid userId, int orderId)
        {
            var order = await _backendContext.Orders
                .FirstAsync(x => x.Id == orderId);

            if (order.Cook is not null || order.Status != OrderStatus.Created)
            {
                throw new BadHttpRequestException("Order has been taken by someone else");
            }

            order.Status = OrderStatus.Cooking;
            order.Cook = userId;

            await _backendContext.SaveChangesAsync();

            _rabbitService.SendMessage(order.CustomerId.ToString(), $"Order [No. {order.Id}] now has status [{order.Status}]");
        }

        public async Task TakeOrderToPackaging(int orderId)
        {
            var order = await _backendContext.Orders
                .FirstAsync(x => x.Id == orderId);

            if (order.Status != OrderStatus.Cooking)
            {
                throw new BadHttpRequestException("Order is being packaged or has been packaged already");
            }

            order.Status = OrderStatus.Packaging;

            await _backendContext.SaveChangesAsync();

            _rabbitService.SendMessage(order.CustomerId.ToString(), $"Order [No. {order.Id}] now has status [{order.Status}]");

        }

        public async Task<OrderPagedDto> GetCookHistory(
            Guid userId,
            int? number,
            DateTime fromDate = default,
            int page = 1
            )
        {
            var orders = await GetUserHistoryQuery(RoleType.Cook, userId, number, fromDate)
                .ToListAsync();

            return CreateOrderPagedResponse(orders, page);
        }

        public async Task<OrderPagedDto> GetCustomerHistory(
            Guid userId,
            int? number,
            DateTime fromDate = default,
            int page = 1,
            bool activeOnly = false
            )
        {
            var orders = await GetUserHistoryQuery(RoleType.Customer, userId, number, fromDate)
                .Where(x => !activeOnly || (x.Status != OrderStatus.Cancelled && x.Status != OrderStatus.Delivered))
                .ToListAsync();

            return CreateOrderPagedResponse(orders, page);
        }

        public async Task<OrderPagedDto> GetCourierHistory(
            Guid userId,
            int? number,
            DateTime fromDate = default,
            int page = 1
            )
        {
            var orders = await GetUserHistoryQuery(RoleType.Courier, userId, number, fromDate)
                .ToListAsync();

            orders = orders
                .Where(x => new List<OrderStatus>() { OrderStatus.Delivering, OrderStatus.Delivered, OrderStatus.Cancelled }.Contains(x.Status))
                .ToList();

            return CreateOrderPagedResponse(orders, page);
        }

        public async Task<OrderPagedDto> GetRestaurantHistory(
            Guid managerId,
            OrderStatus? status,
            OrderSortingType? sortBy,
            int? number,
            int page = 1
            )
        {
            var orders = (IEnumerable<Order>)await _backendContext.Restaurants
                .Where(r => r.Managers.Contains(managerId))
                .Select(r => r.Orders)
                .FirstAsync();

            if (number is not null)
            {
                orders = orders.Where(x => x.Id == number);
            }
            else
            {
                orders = orders
                    .Where(x => x.Status == (status ?? x.Status))
                    .SortByMethod(sortBy);
            }

            return CreateOrderPagedResponse(orders, page);
        }

        public async Task SetOrderAsBeingDelivered(Guid courierId, int orderId)
        {
            var order = await _backendContext.Orders
                .FirstAsync(x => x.Id == orderId);

            if (order.CourierId is not null || order.Status != OrderStatus.ReadyForDelivery)
            {
                throw new BadHttpRequestException($"Can't change status from {order.Status} to ReadyForDelivery");
            }

            order.CourierId = courierId;
            order.Status = OrderStatus.Delivering;

            await _backendContext.SaveChangesAsync();

            _rabbitService.SendMessage(order.CustomerId.ToString(), $"Order [No. {order.Id}] now has status [{order.Status}]");

        }

        public async Task<OrderAvailablePagedDto> GetAvailableForDelivery(Guid userId, OrderSortingType? sortBy, int page = 1)
        {
            var orders = await _backendContext.Orders
                .Where(x => x.Status == OrderStatus.ReadyForDelivery)
                .ToListAsync();

            return CreateOrderAvailablePagedResponse(orders, page);

        }

        public async Task SetOrderAsDelivered(int orderId)
        {
            var order = await _backendContext.Orders
                .FirstAsync(o => o.Id == orderId);

            if (order.Status != OrderStatus.Delivering)
            {
                throw new BadHttpRequestException($"Can't change status from {order.Status} to Delivering");
            }

            order.Status = OrderStatus.Delivered;
            await _backendContext.SaveChangesAsync();

            _rabbitService.SendMessage(order.CustomerId.ToString(), $"Order [No. {order.Id}] now has status [{order.Status}]");

        }

        private static OrderPagedDto CreateOrderPagedResponse(IEnumerable<Order> collection, int page)
        {
            var response = new OrderPagedDto { PageInfo = new PageInfo(collection.Count(), _OrdersPageSize, page) };

            collection = collection
                .Skip(_OrdersPageSize * (page - 1))
                .Take(_OrdersPageSize);

            foreach (var order in collection)
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
        private static OrderAvailablePagedDto CreateOrderAvailablePagedResponse(IEnumerable<Order> collection, int page)
        {
            var response = new OrderAvailablePagedDto() { PageInfo = new PageInfo(collection.Count(), _OrdersPageSize, page) };

            collection = collection
                .Skip(_OrdersPageSize * (page - 1))
                .Take(_OrdersPageSize);

            foreach (var order in collection)
            {
                response.Orders.Add(new OrderShortestDto
                {
                    Id = order.Id,
                    OrderTime = order.OrderTime,
                    DeliveryTime = order.DeliveryTime
                });
            }

            return response;
        }
        private async Task<Order> GetOrder(int orderId)
            => await _backendContext.Orders
                .Include(o => o.Dishes)
                    .ThenInclude(dc => dc.Dish)
                .Include(r => r.Restaurant)
                .FirstAsync(o => o.Id == orderId);
        private IQueryable<Order> GetUserHistoryQuery(
            RoleType role,
            Guid userId,
            int? number = default,
            DateTime fromDate = default
            )
        {
            // определяем предикат фильтрации в зависимости от роли, т.к. суть поиска одинакова, достаточно поменять поля, по которым сравниваем ID пользователей
            Expression<Func<Order, bool>> predicate = role switch
            {
                RoleType.Courier => (Order o) => o.CourierId == userId,
                RoleType.Cook => (Order o) => o.Cook == userId,
                RoleType.Customer => (Order o) => o.CustomerId == userId,
                _ or RoleType.Manager => throw new BadHttpRequestException("Not allowed", StatusCodes.Status403Forbidden)
            };

            var query = _backendContext.Orders
                .Where(predicate)
                .Where(o => o.OrderTime > fromDate);

            if (number != null)
            {
                query = query.Where(o => o.Id == number);
            }

            return query.OrderBy(x => x.OrderTime);
        }
    }

    static class IEnumerableExtension
    {
        public static IEnumerable<Order> SortByMethod(this IEnumerable<Order> orders, OrderSortingType? sortBy)
        {
            return sortBy switch
            {
                OrderSortingType.CREATION_DATE_ASCENDING => orders.OrderBy(x => x.OrderTime),
                OrderSortingType.CREATION_DATE_DESCENDING => orders.OrderByDescending(x => x.OrderTime),
                OrderSortingType.DELIVERY_DATE_ASCENDING => orders.OrderBy(x => x.DeliveryTime),
                OrderSortingType.DELIVERY_DATE_DESCENDING => orders.OrderByDescending(x => x.DeliveryTime),
                _ => orders
            };
        }
    }
}
