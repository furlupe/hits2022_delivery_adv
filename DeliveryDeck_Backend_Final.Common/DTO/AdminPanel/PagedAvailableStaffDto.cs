using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class PagedAvailableStaffDto
    {
        public List<AvailableStaffDto> Users { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
