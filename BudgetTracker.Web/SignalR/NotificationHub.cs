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
    }
}
