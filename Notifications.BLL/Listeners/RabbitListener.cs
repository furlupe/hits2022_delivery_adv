using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Notifications.BLL.Hubs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using DeliveryDeck_Backend_Final.Common.DTO.RabbitMQ;

namespace Notifications.Listeners
{
    public class RabbitListener : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConnection _connection;
        public RabbitListener(IHubContext<NotificationHub> hubContext, IConnection connection)
        {
            _hubContext = hubContext;
            _connection = connection;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: "Notifications",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (sender, args) =>
            {
                var content = JsonSerializer.Deserialize<UserMessage>(Encoding.UTF8.GetString(args.Body.ToArray()));
                if (content != null)
                {
                    await _hubContext.Clients.User(content.Id).SendAsync("ReceiveMessage", content.Message);
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
