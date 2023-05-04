using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace AdminPanel.Models
{
    public class UserListModel
    {
        public List<UserShortModel> Users { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
