namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class PagedDishesDto
    {
        public ICollection<DishDto> Dishes { get; set; } = new List<DishDto>();
        public PageInfo PageInfo { get; set; }
    }
}
