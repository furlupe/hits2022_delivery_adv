using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace AdminPanel.Models
{
    public class RestaurantListModel
    {
        public List<RestaurantShortModel> Restaurants { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
