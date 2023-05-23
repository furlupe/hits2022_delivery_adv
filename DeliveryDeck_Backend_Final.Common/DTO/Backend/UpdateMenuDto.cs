using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class UpdateMenuDto
    {
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
