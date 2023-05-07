namespace AdminPanel.Models
{
    public class AvailableStaffModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public bool AvailableAsManager { get; set; } = true;
        public bool AvailableAsCook { get; set; } = true;
    }
}
