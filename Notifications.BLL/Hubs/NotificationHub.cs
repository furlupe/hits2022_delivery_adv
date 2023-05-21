using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace Notifications.BLL.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
    }
}
