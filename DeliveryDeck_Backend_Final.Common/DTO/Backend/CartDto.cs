namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class CartDto
    {
        public ICollection<DishCartDto> Dishes { get; set; }
        public int Price { get; set; }
        public List<Guid> RemovedDishes { get; set; }
    }
}
