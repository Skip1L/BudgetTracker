using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using BudgetTracker.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BudgetTracker.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly BudgetTrackerContext _context;

        public ExpenseController(BudgetTrackerContext context)
        {
            _context = context;
        }

        // Get all expenses for a budget
        [HttpGet("{budgetId}")]
        public ActionResult<IEnumerable<Expense>> GetExpenses(int budgetId)
        {
            var expenses = _context.Expenses.Where(e => e.BudgetId == budgetId).ToList();
            if (!expenses.Any()) return NotFound();
            return expenses;
        }

        // Create a new expense
        [HttpPost]
        public IActionResult CreateExpense([FromBody] Expense expense)
        {
            var budget = _context.Budgets.FirstOrDefault(b => b.Id == expense.BudgetId);
            if (budget == null) return NotFound("Budget not found");

            _context.Expenses.Add(expense);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetExpenses), new { budgetId = expense.BudgetId }, expense);
        }

        // Update an expense
        [HttpPut("{id}")]
        public IActionResult UpdateExpense(int id, [FromBody] Expense expense)
        {
            var existingExpense = _context.Expenses.FirstOrDefault(e => e.Id == id);
            if (existingExpense == null) return NotFound();

            existingExpense.Description = expense.Description;
            existingExpense.Amount = expense.Amount;
            existingExpense.Date = expense.Date;

            _context.SaveChanges();
            return NoContent();
        }

        // Delete an expense
        [HttpDelete("{id}")]
        public IActionResult DeleteExpense(int id)
        {
            var expense = _context.Expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null) return NotFound();

            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
