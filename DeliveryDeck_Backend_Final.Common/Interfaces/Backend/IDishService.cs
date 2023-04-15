using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IDishService
    {
        Task<DishDto> GetDishById(Guid dishId);
        Task RateDish(Guid userId, Guid dishId, int rating);
    }
}
