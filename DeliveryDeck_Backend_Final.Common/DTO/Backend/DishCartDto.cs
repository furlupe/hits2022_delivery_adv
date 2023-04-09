using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class DishCartDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsVegeterian { get; set; }
        public string? Photo { get; set; }
        public FoodCategory Category;
        public int Amount { get; set; }
    }
}
