using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Utils;
using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO.Auth
{
    public class UserUpdateProfileDto
    {
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [EnumValueDefined]
        public Gender Gender { get; set; }
        [Required]
        [RegularExpression(@"\d{11}")]
        public string PhoneNumber { get; set; }
        [Required]
        [MinLength(1)]
        public string Address { get; set; }
    }
}
