using DeliveryDeck_Backend_Final.Common.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class UserProfileDto
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
