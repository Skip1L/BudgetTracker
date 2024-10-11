using Microsoft.AspNetCore.SignalR;

namespace BudgetTracker.Web.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task NotifyExpenseAdded(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        public async Task SendBudgetExceededNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveBudgetExceededNotification", message);
        }

        // Send a notification to all connected clients
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
