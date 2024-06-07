using Microsoft.AspNetCore.SignalR;

namespace _71BootlegStore.Models
{
    public class SignalServer : Hub
    {
        public async Task SendMessage(Notifications notifications)
        {
            await Clients.All.SendAsync("ReceiveMessage", notifications.user, notifications.message);
        }
    }
}
