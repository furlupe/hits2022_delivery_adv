namespace DeliveryDeck_Backend_Final.Common.Utils
{
    internal static class OrderPermissions
    {
        public const string ReadOwn = "orders.readOwn";
        public const string ReadRestaurant = "orders.readRestaurant";
        public const string Add = "orders.add";
        public const string ChangeStatus = "orders.changeStatus";

        public const string GetAvailableForDelivery = "orders.getAvailableForDelivery";
        public const string GetAvailableForCooking = "orders.getAvailableForCooking";
    }
}
