namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class PagedMenusDto
    {
        public ICollection<MenuDto> Menus { get; set; } = new List<MenuDto>();
        public PageInfo PageInfo { get; set; }
    }
}
