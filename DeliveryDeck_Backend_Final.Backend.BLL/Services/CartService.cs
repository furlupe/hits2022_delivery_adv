using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly BackendContext _backendContext;
        public CartService(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }
        public async Task AddDish(Guid userId, Guid dishId, int amount = 1)
        {
            var cart = await GetCartByUserId(userId);

            var existingDish = cart.Dishes
                .FirstOrDefault(d => d.Dish.Id == dishId);

            if (existingDish != null)
            {
                existingDish.Amount += amount;
            }
            else
            {
                var dish = await _backendContext.Dishes.FirstAsync(d => d.Id == dishId);

                cart.Dishes.Add(new DishInCart
                {
                    Dish = dish,
                    Amount = amount
                });
            }

            await _backendContext.SaveChangesAsync();
        }
        public async Task<CartDto> GetCart(Guid userId)
        {
            var cart = await GetCartByUserId(userId);

            var dishes = new List<DishCartDto>();
            var removed = new List<Guid>();
            var totalPrice = 0;
            foreach (var dishInCart in cart.Dishes)
            {
                var dish = await _backendContext.Dishes
                    .FirstAsync(d => d == dishInCart.Dish);

                if (await _backendContext.Menus
                    .Where(m => m.Dishes.Contains(dish))
                    .AllAsync(m => m.IsActive == false)
                    )
                {                    
                    removed.Add(dish.Id);
                    continue;
                }

                dishes.Add(new DishCartDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    IsVegeterian = dish.IsVegeterian,
                    Photo = dish.Photo,
                    Category = dish.Category,
                    Amount = dishInCart.Amount
                });

                totalPrice += dishInCart.Amount * dish.Price;
            }

            return new CartDto
            {
                Dishes = dishes,
                Price = totalPrice,
                RemovedDishes = removed
            };
        }
        public async Task RemoveDish(Guid userId, Guid dishId, int amount = 1)
        {
            var cart = await GetCartByUserId(userId);

            var dishInCart = cart.Dishes.First(d => d.Dish.Id == dishId);

            dishInCart.Amount -= amount;

            if (dishInCart.Amount <= 0)
            {
                cart.Dishes.Remove(dishInCart);
            }

            await _backendContext.SaveChangesAsync();
        }
        public async Task RemoveDishCompletely(Guid userId, Guid dishId)
        {
            var cart = await GetCartByUserId(userId);

            var dishInCart = cart.Dishes.First(d => d.Dish.Id == dishId);

            cart.Dishes.Remove(dishInCart);

            await _backendContext.SaveChangesAsync();
        }

        private async Task<Cart> GetCartByUserId(Guid customerId)
        {
            return await _backendContext.Carts
                .Include(c => c.Dishes)
                    .ThenInclude(d => d.Dish)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.WasOrdered == false)
                ?? await CreateCart(customerId);
        }
        private async Task<Cart> CreateCart(Guid customerId)
        {
            var cart = new Cart
            {
                CustomerId = customerId
            };

            await _backendContext.Carts.AddAsync(cart);
            await _backendContext.SaveChangesAsync();

            return cart;
        }
    }
}
