using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Listeners;
using Notifications.Providers;
using RabbitMQ.Client;

namespace Notifications.BLL.Extensions
{
    public static class NotificationsExtension
    {
        public static WebApplicationBuilder AddNotificationsListener(this WebApplicationBuilder builder, string brokerHostName)
        {
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            builder.Services.AddHostedService<RabbitListener>();
            builder.Services.AddSingleton(x => new ConnectionFactory { HostName = brokerHostName }.CreateConnection());

            return builder;
        }
    }
}
