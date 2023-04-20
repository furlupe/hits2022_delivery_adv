using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class DishFilters
    {
        public ICollection<FoodCategory> Categories { get; set; }
        public Guid? Menu { get; set; }
        public bool? IsVegetarian { get; set; }
        public DishSortingType? SortBy { get; set; }
    }
}
