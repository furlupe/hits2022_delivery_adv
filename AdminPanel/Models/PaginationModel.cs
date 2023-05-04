using DeliveryDeck_Backend_Final.Common.DTO.Backend;

namespace AdminPanel.Models
{
    public class PaginationModel
    {
        public PageInfo PageInfo { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; } = "Index";
    }
}
