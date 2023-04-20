namespace DeliveryDeck_Backend_Final.Auth.DAL.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public AppUser User { get; set; }
        public string Address { get; set; }
    }
}
