using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using BudgetTracker.Web.SignalR;

namespace BudgetTracker.Web.Services
{
    public class BudgetNotificationService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<NotificationHub> _hubContext;
        private Timer _timer;

        public BudgetNotificationService(IServiceProvider serviceProvider, IHubContext<NotificationHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Schedule the method to run periodically (e.g., every 10 minutes)
            _timer = new Timer(CheckBudgetThresholds, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            return Task.CompletedTask;
        }

        private void CheckBudgetThresholds(object state)
        {
            // Create a new scope to resolve the scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                var budgetService = scope.ServiceProvider.GetRequiredService<BudgetService>();

                // Example: check if any budgets exceed the threshold
                var budgets = budgetService.GetAllBudget();
                foreach (var budget in budgets)
                {
                    if (budgetService.IsBudgetExceeded(budget.Id))
                    {
                        // Send notification using SignalR when a budget exceeds the limit
                        _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Budget '{budget.Name}' has exceeded the limit.");
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
