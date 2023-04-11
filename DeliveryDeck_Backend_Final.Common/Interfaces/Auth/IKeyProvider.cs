namespace DeliveryDeck_Backend_Final.Common.Interfaces.Auth
{
    public interface IKeyProvider
    {
        string CreateKey(int length);
    }
}
