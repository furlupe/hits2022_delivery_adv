namespace DeliveryDeck_Backend_Final.Common.Interfaces.Backend
{
    public interface IResourceAuthorizationService
    {
        Task<bool> DishResourceExists(Guid dishId);
        Task<bool> DishInCartResourceExists(Guid userId, Guid dishId);
        Task<bool> DishIsActive(Guid dishId);

        Task<bool> OrderCustomerRelationExists(Guid userId, int orderId);

        Task<bool> MenuResourceExists(Guid menuId);

        Task<bool> RestaurantResourceExists(Guid restaurantId);
        Task<bool> ManagerRestaurantMenuResourceExists(Guid manager, Guid menuId);
        Task<bool> ManagerRestaurantDishResourceExists(Guid manager, Guid dishId);

        Task<bool> StaffRestaurantOrderResourceExists(Guid userId, int orderId);
        Task<bool> OrderCookRelationExists(Guid cookId, int orderId);

        Task<bool> OrderIsAccessibleForCourier(Guid courierId, int orderId);
        Task<bool> OrderCourierRelationExists(Guid courierId, int orderId);

    }
}
