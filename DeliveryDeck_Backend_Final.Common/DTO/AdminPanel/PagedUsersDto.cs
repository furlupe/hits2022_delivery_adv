using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class PagedUsersDto
    {
        public List<UserShortDto> Users { get; set; } = new();
        public PageInfo PageInfo { get; set; }
    }
}
