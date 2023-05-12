using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Notifications.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
            => await Clients.All.SendAsync("RecieveMessage", message);
    }
}
