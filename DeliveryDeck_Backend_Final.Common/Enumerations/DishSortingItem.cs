namespace DeliveryDeck_Backend_Final.Common.Enumerations
{
    public enum DishSortingItem
    {
        name,
        rating,
        price
    }

    public static class SortingItemMethods
    {
        public static DishSortingType? ToSortingType(this DishSortingItem? item, bool isDescending = false)
        {
            if (isDescending)
            {
                return item switch
                {
                    DishSortingItem.name => DishSortingType.NAME_DESCENDING,
                    DishSortingItem.price => DishSortingType.PRICE_DESCENDING,
                    DishSortingItem.rating => DishSortingType.RATING_DESCENDING,
                    _ => null
                };
            }

            return item switch
            {
                DishSortingItem.name => DishSortingType.NAME_ASCENDING,
                DishSortingItem.price => DishSortingType.PRICE_ASCENDING,
                DishSortingItem.rating => DishSortingType.RATING_ASCENDING,
                _ => null
            };
        }
    }
}
