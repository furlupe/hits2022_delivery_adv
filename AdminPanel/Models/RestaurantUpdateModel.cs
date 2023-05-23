using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class RestaurantUpdateModel
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
    }
}
