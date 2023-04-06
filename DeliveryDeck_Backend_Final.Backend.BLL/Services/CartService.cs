using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using Microsoft.AspNetCore.Http;
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
                .SingleOrDefault(d => d.Dish.Id == dishId);

            if(existingDish != null)
            {
                existingDish.Amount += amount;
            } 
            else
            {
                cart.Dishes.Add(new DishInCart
                {
                    Dish = await _backendContext.Dishes.SingleAsync(d => d.Id == dishId),
                    Amount = amount
                });
            }

            await _backendContext.SaveChangesAsync();
        }

        public async Task<CartDto> GetCart(Guid userId)
        {
            var cart = await GetCartByUserId(userId);

            var dishes = new List<DishCartDto>();
            var totalPrice = 0;
            foreach(var dishInCart in cart.Dishes)
            {
                var dish = await _backendContext.Dishes.SingleAsync(d => d == dishInCart.Dish);

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
                Price = totalPrice
            };
        }

        public async Task RemoveDish(Guid userId, Guid dishId, int amount = 1)
        {
            var cart = await GetCartByUserId(userId);

            var dishInCart = cart.Dishes.SingleOrDefault(d => d.Dish.Id == dishId)
                ?? throw new BadHttpRequestException("Dish is not in the cart already, moron");

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

            var dishInCart = cart.Dishes.SingleOrDefault(d => d.Dish.Id == dishId)
                ?? throw new BadHttpRequestException("Dish is not in the cart already, moron");

            cart.Dishes.Remove(dishInCart);

            await _backendContext.SaveChangesAsync();
        }

        private async Task<Cart> GetCartByUserId(Guid customerId)
        {
            return await _backendContext.Carts
                .Include(c => c.Dishes)
                    .ThenInclude(d => d.Dish)
                .SingleOrDefaultAsync(c => c.CustomerId == customerId && c.Order == null)
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
