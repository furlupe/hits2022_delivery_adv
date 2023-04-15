namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IResourceAuthorizationService
    {
        Task<bool> DishResourceExists(Guid dishId);
        Task<bool> DishInCartResourceExists(Guid userId, Guid dishId);

        Task<bool> OrderResourceExists(Guid userId, int orderId);

        Task<bool> MenuResourceExists(Guid menuId);

        Task<bool> RestaurantResourceExists(Guid restaurantId);
        Task<bool> RestaurantMenuResourceExists(Guid manager, Guid menuId);
        Task<bool> RestaurantDishResourceExists(Guid manager, Guid dishId);

        Task<bool> RestaurantOrderExists(Guid userId, int orderId);
        Task<bool> OrderCookRelationExists(Guid cookId, int orderId);
    }
}
