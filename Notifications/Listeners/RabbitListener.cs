using DeliveryDeck_Backend_Final.Common.DTO.RabbitMQ;
using Microsoft.AspNetCore.SignalR;
using Notifications.Hubs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Notifications.Listeners
{
    public class RabbitListener : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public RabbitListener(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "Notifications",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var content = JsonSerializer.Deserialize<UserMessage>(Encoding.UTF8.GetString(args.Body.ToArray()));
                if (content != null)
                {
                    _hubContext.Clients.User(content.Id).SendAsync("ReceiveMessage", content.Message);
                }
            };

            channel.BasicConsume(
                queue: "Notifications",
                autoAck: true,
                consumer: consumer
                );

            return Task.CompletedTask;
        }
    }
}
