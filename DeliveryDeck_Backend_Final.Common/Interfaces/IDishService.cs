using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces
{
    public interface IDishService
    {
        Task<DishDto> GetDishById(Guid dishId);
    }
}
