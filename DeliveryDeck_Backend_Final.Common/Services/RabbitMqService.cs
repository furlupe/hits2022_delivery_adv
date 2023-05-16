using DeliveryDeck_Backend_Final.Common.DTO.RabbitMQ;
using DeliveryDeck_Backend_Final.Common.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DeliveryDeck_Backend_Final.Common
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        public RabbitMqService(IConnection connection)
        {
            _connection = connection;
        }
        public void SendMessage(string userId, string message)
        {
            var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: "Notifications",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var body = new UserMessage
            {
                Message = message,
                Id = userId
            };

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "Notifications",
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(body))
                );
        }
    }
}
