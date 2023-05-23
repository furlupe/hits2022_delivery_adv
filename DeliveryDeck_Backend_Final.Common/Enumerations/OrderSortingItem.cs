namespace DeliveryDeck_Backend_Final.Common.Enumerations
{
    public enum OrderSortingItem
    {
        creation_date,
        delivery_date
    }

    public static class OrderSortingItemMethods
    {
        public static OrderSortingType? ToSortingType(this OrderSortingItem? item, bool isDescending = false)
        {
            if (isDescending)
            {
                return item switch
                {
                    OrderSortingItem.creation_date => OrderSortingType.CREATION_DATE_DESCENDING,
                    OrderSortingItem.delivery_date => OrderSortingType.CREATION_DATE_DESCENDING,
                    _ => null
                };
            }

            return item switch
            {
                OrderSortingItem.creation_date => OrderSortingType.CREATION_DATE_ASCENDING,
                OrderSortingItem.delivery_date => OrderSortingType.DELIVERY_DATE_ASCENDING,
                _ => null
            };
        }
    }
}
