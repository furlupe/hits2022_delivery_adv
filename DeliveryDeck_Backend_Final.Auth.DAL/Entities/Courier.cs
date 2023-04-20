namespace DeliveryDeck_Backend_Final.Auth.DAL.Entities
{
    public class Courier
    {
        public Guid Id { get; set; }
        public AppUser User { get; set; }
    }
}
