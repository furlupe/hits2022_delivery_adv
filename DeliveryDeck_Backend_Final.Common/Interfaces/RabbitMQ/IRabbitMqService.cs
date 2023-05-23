namespace DeliveryDeck_Backend_Final.Common.Interfaces.RabbitMQ
{
    public interface IRabbitMqService
    {
        void SendMessage(string userId, string message);
    }
}
