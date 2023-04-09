using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCart(Guid userId);
        Task AddDish(Guid userId, Guid dishId, int amount = 1);
        Task RemoveDish(Guid userId, Guid dishId, int amount = 1);
        Task RemoveDishCompletely(Guid userId, Guid dishId);
    }
}
