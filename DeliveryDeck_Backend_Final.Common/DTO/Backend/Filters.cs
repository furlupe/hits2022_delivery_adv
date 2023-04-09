using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class Filters
    {
        public ICollection<FoodCategory> Categories { get; set; }
        public string? Menu { get; set; }
        public bool? IsVegetarian { get; set; }
        public SortingType? SortBy { get; set; }
    }
}
