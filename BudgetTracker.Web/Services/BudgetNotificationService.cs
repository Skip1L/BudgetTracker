using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using BudgetTracker.Web.SignalR;

namespace BudgetTracker.Web.Services
{
    public class BudgetNotificationService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly BudgetService _budgetService;

        public BudgetNotificationService(IHubContext<NotificationHub> hubContext, BudgetService budgetService)
        {
            _hubContext = hubContext;
            _budgetService = budgetService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Simulate checking all budgets (here only budget with ID 1 is checked)
                if (_budgetService.IsBudgetExceeded(1))
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveBudgetExceededNotification", "Budget limit exceeded!", cancellationToken: stoppingToken);
                }

                await Task.Delay(10000, stoppingToken);  // Check every 10 seconds
            }
        }
    }
}
