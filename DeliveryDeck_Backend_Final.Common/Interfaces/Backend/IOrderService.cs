using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IOrderService
    {
        Task<RemovedDishesDto> CreateOrder(Guid userId, CreateOrderDto data);
        Task CancelOrder(int orderId);
        Task<OrderDto> GetOrderDetails(int orderId);
        Task<RemovedDishesDto> RepeatPreviousOrder(int orderId);

        Task<OrderPagedDto> GetCookHistory(Guid userId, int? number, DateTime fromDate = default, int page = 1);
        Task<OrderPagedDto> GetCustomerHistory(Guid userId, int? number, DateTime fromDate = default, int page = 1, bool activeOnly = false);
        Task<OrderPagedDto> GetCourierHistory(Guid userId, int? number, DateTime fromDate = default, int page = 1);
        Task<OrderPagedDto> GetRestaurantHistory(Guid managerId, OrderStatus? status, OrderSortingType? sortBy, int? number, int page = 1);

        Task<OrderAvailablePagedDto> GetAvailableForKitchen(Guid userId, OrderSortingType? sortBy, int page = 1);
        Task TakeOrderToKitchen(Guid userId, int orderId);
        Task TakeOrderToPackaging(int orderId);
        Task SetOrderToDeliveryAvailable(int orderId);

        Task<OrderAvailablePagedDto> GetAvailableForDelivery(Guid userId, OrderSortingType? sortBy, int page = 1);
        Task SetOrderAsBeingDelivered(Guid courierId, int orderId);
        Task SetOrderAsDelivered(int orderId);
    }
}
