namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class PagedDishesDto
    {
        public ICollection<DishShortDto> Dishes { get; set; } = new List<DishShortDto>();
        public PageInfo PageInfo { get; set; }
    }
}
