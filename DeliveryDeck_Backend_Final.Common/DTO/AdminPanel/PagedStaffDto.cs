using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class PagedStaffDto
    {
        public List<StaffDto> Staff { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
