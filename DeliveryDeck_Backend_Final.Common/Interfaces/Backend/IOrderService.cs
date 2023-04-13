using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IOrderService
    {
        Task<List<Guid>> CreateOrder(Guid userId, CreateOrderDto data);
        Task CancelOrder(Guid userId, int orderId);
        Task<OrderDto> GetOrderDetails(Guid userId, int orderId);
        Task<OrderPagedDto> GetHistory(Guid userId, int? number, DateTime fromDate = default, int page = 1, bool activeOnly = false);

        Task ChangeOrderStatus(Guid userId, int orderId, OrderStatus status);
    }
}
