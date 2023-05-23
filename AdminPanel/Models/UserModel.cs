using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace AdminPanel.Models
{
    public class UserModel : UserShortModel
    {
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }

        public RestaurantShortModel? RestaurantAsManager { get; set; }
        public RestaurantShortModel? RestaurantAsCook { get; set; }

    }
}
