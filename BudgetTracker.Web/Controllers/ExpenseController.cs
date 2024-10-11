// Controllers/ExpenseController.cs
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
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ExpenseController(IExpenseRepository expenseRepository, IHubContext<NotificationHub> hubContext)
        {
            _expenseRepository = expenseRepository;
            _hubContext = hubContext;
        }

        // Get all expenses for a budget
        [HttpGet("budget/{budgetId}")]
        public ActionResult<IEnumerable<Expense>> GetExpenses(int budgetId)
        {
            var expenses = _expenseRepository.GetAllExpenses(budgetId);
            return Ok(expenses);
        }

        // Get expense by ID
        [HttpGet("{id}")]
        public ActionResult<Expense> GetExpense(int id)
        {
            var expense = _expenseRepository.GetExpenseById(id);
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        // Create a new expense
        [HttpPost]
        public IActionResult CreateExpense([FromBody] Expense expense)
        {
            _expenseRepository.AddExpense(expense);
            // Send a notification when an expense is created
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Expense '{expense.Description}' has been added with amount {expense.Amount}.");

            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }

        // Update expense
        [HttpPut("{id}")]
        public IActionResult UpdateExpense(int id, [FromBody] Expense expense)
        {
            var existingExpense = _expenseRepository.GetExpenseById(id);
            if (existingExpense == null) return NotFound();

            // Send a notification when an expense is updated
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Expense '{expense.Description}' has been updated.");


            _expenseRepository.UpdateExpense(expense);
            return NoContent();
        }

        // Delete expense
        [HttpDelete("{id}")]
        public IActionResult DeleteExpense(int id)
        {
            var expense = _expenseRepository.GetExpenseById(id);
            if (expense == null) return NotFound();

            // Send a notification when an expense is deleted
            _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Expense '{expense.Description}' has been deleted.");


            _expenseRepository.DeleteExpense(id);
            return NoContent();
        }
    }
}
