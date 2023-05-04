namespace AdminPanel.Models
{
    public class RestaurantModel : RestaurantShortModel
    {
        public List<Guid> Managers { get; set; } = new();
        public List<Guid> Cooks { get; set; } = new();
    }
}
