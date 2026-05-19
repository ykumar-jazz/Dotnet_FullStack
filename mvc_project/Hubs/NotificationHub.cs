using Microsoft.AspNetCore.SignalR;

namespace mvc_project.EMS.Web.Hubs;

public class NotificationHub : Hub
{
   public async Task SendNotification(
        string message)
    {
        await Clients.All.SendAsync(
            "ReceiveNotification",
            message);
    }
}