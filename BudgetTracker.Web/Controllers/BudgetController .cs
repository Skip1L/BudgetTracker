using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly BudgetTrackerContext _context;

        public BudgetController(BudgetTrackerContext context)
        {
            _context = context;
        }

        // Get all budgets
        [HttpGet]
        public ActionResult<IEnumerable<Budget>> GetBudgets()
        {
            return _context.Budgets.Include(b => b.Expenses).ToList(); // Include expenses
        }

        // Get budget by ID
        [HttpGet("{id}")]
        public ActionResult<Budget> GetBudget(int id)
        {
            var budget = _context.Budgets.Include(b => b.Expenses).FirstOrDefault(b => b.Id == id);
            if (budget == null) return NotFound();
            return budget;
        }

        [HttpPost]
        public IActionResult CreateBudget([FromBody] Budget budget)
        {
            _context.Budgets.Add(budget);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetBudgets), new { id = budget.Id }, budget);
        }

        // Update budget
        [HttpPut("{id}")]
        public IActionResult UpdateBudget(int id, [FromBody] Budget budget)
        {
            var existingBudget = _context.Budgets.FirstOrDefault(b => b.Id == id);
            if (existingBudget == null) return NotFound();

            existingBudget.Name = budget.Name;
            existingBudget.TotalAmount = budget.TotalAmount;

            _context.SaveChanges();
            return NoContent();
        }

        // Delete budget
        [HttpDelete("{id}")]
        public IActionResult DeleteBudget(int id)
        {
            var budget = _context.Budgets.FirstOrDefault(b => b.Id == id);
            if (budget == null) return NotFound();

            _context.Budgets.Remove(budget);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
