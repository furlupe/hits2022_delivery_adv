using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace AdminPanel.Models
{
    public class StaffListModel
    {
        public List<StaffModel> Staff { get; set; }
        public PageInfo PageInfo { get; set; }

    }
}
