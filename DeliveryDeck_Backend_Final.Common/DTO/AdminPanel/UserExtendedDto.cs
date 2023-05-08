using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class UserExtendedDto : UserShortDto
    {
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }

        public string? Address { get; set; }

        public RestaurantShortDto? RestaurantAsManager { get; set; }
        public RestaurantShortDto? RestaurantAsCook { get; set; }

    }
}
