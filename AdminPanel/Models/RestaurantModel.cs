using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class RestaurantModel
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public List<Guid> Managers { get; set; } = new();
        public List<Guid> Cooks { get; set; } = new();
    }
}
