// Controllers/IncomeController.cs
using BudgetTracker.Web.Repositories;
using BudgetTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BudgetTracker.Web.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace BudgetTracker.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public IncomeController(IIncomeRepository incomeRepository, IHubContext<NotificationHub> hubContext)
        {
            _incomeRepository = incomeRepository;
            _hubContext = hubContext;
        }

        // Get all incomes for a budget
        [HttpGet("budget/{budgetId}")]
        public ActionResult<IEnumerable<Income>> GetIncomes(int budgetId)
        {
            var incomes = _incomeRepository.GetAllIncomes(budgetId);
            return Ok(incomes);
        }

        // Get income by ID
        [HttpGet("{id:int}")]
        public ActionResult<Income> GetIncome(int id)
        {
            var income = _incomeRepository.GetIncomeById(id);
            if (income == null) return NotFound();
            return Ok(income);
        }

        // Create a new income
        [HttpPost]
        public IActionResult CreateIncome([FromBody] Income income)
        {
            _incomeRepository.AddIncome(income);

            // Send a notification when an income is created
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Income '{income.Source}' of {income.Amount} has been added.");

            return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, income);
        }

        // Update income
        [HttpPut("{id:int}")]
        public IActionResult UpdateIncome(int id, [FromBody] Income income)
        {
            var existingIncome = _incomeRepository.GetIncomeById(id);
            if (existingIncome == null) return NotFound();

            // Send a notification when an income is updated
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Income '{income.Source}' has been updated.");

            _incomeRepository.UpdateIncome(income);
            return NoContent();
        }

        // Delete income
        [HttpDelete("{id:int}")]
        public IActionResult DeleteIncome(int id)
        {
            var income = _incomeRepository.GetIncomeById(id);
            if (income == null) return NotFound();

            // Send a notification when an income is deleted
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Income '{income.Source}' has been deleted.");

            _incomeRepository.DeleteIncome(id);
            return NoContent();
        }
    }
}
