using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                .SingleOrDefaultAsync(d => d.Id == dishId)
                ?? throw new BadHttpRequestException("As a matter of fact, there is no such dish with an identifyer you've chosen to pass");

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
    }
}
