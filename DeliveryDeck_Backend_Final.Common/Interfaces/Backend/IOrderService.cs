using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IOrderService
    {
        Task<RemovedDishesDto> CreateOrder(Guid userId, CreateOrderDto data);
        Task CancelOrder(Guid userId, int orderId);
        Task<OrderDto> GetOrderDetails(Guid userId, int orderId);
        Task<OrderPagedDto> GetCustomerHistory(Guid userId, int? number, DateTime fromDate = default, int page = 1, bool activeOnly = false);
    }
}
