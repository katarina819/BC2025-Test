using Microsoft.AspNetCore.SignalR;

namespace BootcampApp.SignalR.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotificationToUser(string userId, string message)
        {
            Console.WriteLine($"Sending notification to user {userId}: {message}");
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
            Console.WriteLine("Notification sent.");
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("SignalR connected: " + Context.UserIdentifier);
            return base.OnConnectedAsync();
        }



    }
}
