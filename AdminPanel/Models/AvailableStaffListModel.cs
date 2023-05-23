using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace AdminPanel.Models
{
    public class AvailableStaffListModel
    {
        public List<AvailableStaffModel> Users { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
