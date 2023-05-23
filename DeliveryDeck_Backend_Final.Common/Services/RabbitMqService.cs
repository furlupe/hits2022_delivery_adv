using DeliveryDeck_Backend_Final.Common.DTO.RabbitMQ;
using DeliveryDeck_Backend_Final.Common.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DeliveryDeck_Backend_Final.Common
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IModel _channel;
        public RabbitMqService(IConnection connection)
        {
            _channel = connection.CreateModel();
        }
        public void SendMessage(string userId, string message)
        {

            _channel.QueueDeclare(
                queue: "Notifications",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var body = new UserMessage
            {
                Message = message,
                Id = userId
            };

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "Notifications",
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(body))
                );
        }
    }
}
