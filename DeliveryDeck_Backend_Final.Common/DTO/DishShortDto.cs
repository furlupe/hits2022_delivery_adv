using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class DishShortDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public bool IsVegeterian { get; set; }
        public string? Photo { get; set; }
        public FoodCategory Category { get; set; }
    }
}
