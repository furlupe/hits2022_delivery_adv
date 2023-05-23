namespace DeliveryDeck_Backend_Final.Common.DTO.Auth
{
    public class TokenPairDto
    {
        public Token AccessToken { get; set; }
        public Token RefreshToken { get; set; }
    }
}
