using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Entities;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Services
{
    public class DishService : IDishService
    {
        private readonly BackendContext _backendContext;
        public DishService(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }

        public async Task<DishDto> GetDishById(Guid dishId)
        {
            var dish = await _backendContext.Dishes
                .Include(d => d.Ratings)
                .FirstAsync(d => d.Id == dishId);

            return new DishDto
            {
                Id = dishId,
                Name = dish.Name,
                Price = dish.Price,
                Rating = dish.Rating,
                Description = dish.Description,
                IsVegeterian = dish.IsVegeterian,
                Photo = dish.Photo,
                Category = dish.Category
            };
        }

        public async Task RateDish(Guid userId, Guid dishId, int rating)
        {
            if (rating < 1 || rating > 10)
            {
                throw new BadHttpRequestException("Rating should be in a range from 1 to 10");
            }

            var dish = await _backendContext.Dishes
                .Include(d => d.Ratings)
                .FirstAsync(d => d.Id == dishId);

            if (await _backendContext.Orders
                .FirstOrDefaultAsync(
                    x =>
                        x.CustomerId == userId
                        && x.Dishes.Select(y => y.Dish).Contains(dish)
                        && x.Status == OrderStatus.Delivered) is null)
            {
                throw new BadHttpRequestException("Uh-oh, you haven't ordered that dish yet...");
            }

            var existingRating = new Rating();
            if ((existingRating = dish.Ratings.FirstOrDefault(r => r.AuthorId == userId)) is not null)
            {
                existingRating.Value = rating;
            }
            else
            {
                dish.Ratings.Add(new Rating
                {
                    Dish = dish,
                    AuthorId = userId,
                    Value = rating
                });
            }

            await _backendContext.SaveChangesAsync();

        }
    }
}
