using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class RestaurantCreateModel
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
    }
}
