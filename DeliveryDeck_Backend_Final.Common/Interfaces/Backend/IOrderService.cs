using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IOrderService
    {
        Task CreateOrder(Guid userId, CreateOrderDto data);
    }
}
