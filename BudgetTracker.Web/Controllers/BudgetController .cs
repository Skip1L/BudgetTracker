// Controllers/BudgetController.cs
using BudgetTracker.Web.Repositories;
using BudgetTracker.Web.Models;
using BudgetTracker.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace BudgetTracker.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public BudgetController(IBudgetRepository budgetRepository, IHubContext<NotificationHub> hubContext)
        {
            _budgetRepository = budgetRepository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Budget>> GetBudgets()
        {
            return Ok(_budgetRepository.GetAllBudgets());
        }

        [HttpGet("{id}")]
        public ActionResult<Budget> GetBudget(int id)
        {
            var budget = _budgetRepository.GetBudgetById(id);
            if (budget == null) return NotFound();
            return Ok(budget);
        }

        [HttpPost]
        public IActionResult CreateBudget([FromBody] Budget budget)
        {
            _budgetRepository.AddBudget(budget);

            // Send a notification when a budget is created
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Budget '{budget.Name}' has been created.");

            return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBudget(int id, [FromBody] Budget budget)
        {
            var existingBudget = _budgetRepository.GetBudgetById(id);
            if (existingBudget == null) return NotFound();

            _budgetRepository.UpdateBudget(budget);

            // Send a notification when a budget is updated
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Budget '{budget.Name}' has been updated.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBudget(int id)
        {
            var budget = _budgetRepository.GetBudgetById(id);
            if (budget == null) return NotFound();

            _budgetRepository.DeleteBudget(id);

            // Send a notification when a budget is deleted
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Budget '{budget.Name}' has been deleted.");

            return NoContent();
        }
    }
}
