namespace DeliveryDeck_Backend_Final.Common.CustomPermission
{
    public static class OrderPermissions
    {
        public const string ReadOwnOrderHistory = "orders.readOwnOrderHistory";
        public const string ReadOwnDeliveryHistory = "orders.readOwnDeliveryHistory";
        public const string ReadOwnCookingHistory = "orders.readOwnCookingHistory";
        public const string ReadRestaurant = "orders.readRestaurant";

        public const string Add = "orders.add";

        public const string ChangeStatusUntilDelivery = "orders.changeStatusUntilDelivery";
        public const string Cancel = "orders.cancel";
        public const string ChangeStatusToDelivered = "orders.changeStatusToDelivered";

        public const string GetAvailableForDelivery = "orders.getAvailableForDelivery";
        public const string GetAvailableForCooking = "orders.getAvailableForCooking";
    }
}
