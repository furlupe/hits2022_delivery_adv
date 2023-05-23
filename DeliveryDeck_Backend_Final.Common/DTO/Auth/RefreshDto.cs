using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Auth
{
    public class RefreshDto
    {
        [Required]
        public string Value { get; set; }
    }

}
