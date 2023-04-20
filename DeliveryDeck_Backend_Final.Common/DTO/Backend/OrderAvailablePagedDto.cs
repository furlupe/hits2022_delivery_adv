namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class OrderAvailablePagedDto
    {
        public List<OrderShortestDto> Orders { get; set; } = new();
        public PageInfo PageInfo { get; set; }
    }
}
