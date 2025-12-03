using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WMS_WEBAPI.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public static async Task SendNotificationToUser(IHubContext<NotificationHub> hubContext, string userId, object payload)
        {
            await hubContext.Clients.User(userId).SendAsync("ReceiveNotification", payload);
        }

        public static async Task SendNotificationToAll(IHubContext<NotificationHub> hubContext, object payload)
        {
            await hubContext.Clients.All.SendAsync("ReceiveNotification", payload);
        }
    }
}
