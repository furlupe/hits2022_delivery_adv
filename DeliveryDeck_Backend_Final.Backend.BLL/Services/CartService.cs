using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Interfaces;
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
            var customer = await _backendContext.Customers
                .Include(c => c.Cart)
                .SingleAsync(c => c.Id == userId);

            customer.Cart.Dishes.Add(new DishInCart
            {
                Dish = await _backendContext.Dishes.SingleAsync(d => d.Id == dishId),
                Amount = amount
            });

            await _backendContext.SaveChangesAsync();
        }

        public async Task<CartDto> GetCart(Guid userId)
        {
            var customer = await _backendContext.Customers
                .Include(c => c.Cart)
                    .ThenInclude(cart => cart.Dishes)
                .SingleAsync(c => c.Id == userId);

            var response = new CartDto();
            var totalPrice = 0;
            foreach(var dishInCart in customer.Cart.Dishes)
            {
                var dish = await _backendContext.Dishes.SingleAsync(d => d == dishInCart.Dish);

                response.Dishes.Add(new DishShortDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    IsVegeterian = dish.IsVegeterian,
                    Photo = dish.Photo,
                    Category = dish.Category
                });

                totalPrice += dishInCart.Amount * dish.Price;
            }

            response.Price = totalPrice;
            return response;
        }

        public async Task RemoveDish(Guid userId, Guid dishId, int amount = 1)
        {
            var customer = await _backendContext.Customers
                .Include(c => c.Cart)
                .SingleAsync(c => c.Id == userId);

            var dishInCart = customer.Cart.Dishes.Single(d => d.Dish.Id == dishId);
            dishInCart.Amount -= amount;

            await _backendContext.SaveChangesAsync();
        }

        public async Task RemoveDishCompletely(Guid userId, Guid dishId)
        {
            var customer = await _backendContext.Customers
                .Include(c => c.Cart)
                .SingleAsync(c => c.Id == userId);

            var dishInCart = customer.Cart.Dishes.Single(d => d.Dish.Id == dishId);
            customer.Cart.Dishes.Remove(dishInCart);

            await _backendContext.SaveChangesAsync();
        }
    }
}
