namespace AdminPanel.Models
{
    public class RestaurantModel : RestaurantShortModel
    {
        public List<StaffModel> Managers { get; set; } = new();
        public List<StaffModel> Cooks { get; set; } = new();
    }
}
