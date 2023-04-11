using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IOrderService
    {
        Task CreateOrder(Guid userId, CreateOrderDto data);
        Task CancelOrder(Guid userId, Guid orderId);
        Task<OrderPagedDto> GetHistory(Guid userId, int page = 1, bool activeOnly = false);
    }
}
