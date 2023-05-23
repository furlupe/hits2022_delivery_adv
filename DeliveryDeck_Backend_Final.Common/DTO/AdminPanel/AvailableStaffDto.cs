namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class AvailableStaffDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public bool AvailableAsManager { get; set; } = true;
        public bool AvailableAsCook { get; set; } = true;
    }
}
