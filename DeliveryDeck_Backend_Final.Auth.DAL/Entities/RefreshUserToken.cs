using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Auth.DAL.Entities
{
    public class RefreshUserToken
    {
        [Key]
        public string Value { get; set; }
        public AppUser User { get; set; }
    }
}
