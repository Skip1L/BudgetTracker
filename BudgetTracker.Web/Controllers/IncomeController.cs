// Controllers/IncomeController.cs
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetTracker.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly BudgetTrackerContext _context;

        public IncomeController(BudgetTrackerContext context)
        {
            _context = context;
        }

        // Get all incomes for a budget
        [HttpGet("{budgetId}")]
        public ActionResult<IEnumerable<Income>> GetIncomes(int budgetId)
        {
            return _context.Incomes.Where(i => i.BudgetId == budgetId).ToList();
        }

        // Get income by ID
        [HttpGet("{id:int}")]
        public ActionResult<Income> GetIncome(int id)
        {
            var income = _context.Incomes.Find(id);
            if (income == null) return NotFound();
            return income;
        }

        // Create a new income
        [HttpPost]
        public IActionResult CreateIncome([FromBody] Income income)
        {
            if (income == null)
                return BadRequest();

            _context.Incomes.Add(income);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, income);
        }

        // Update an existing income
        [HttpPut("{id:int}")]
        public IActionResult UpdateIncome(int id, [FromBody] Income updatedIncome)
        {
            // Find the existing income by ID
            var existingIncome = _context.Incomes.Find(id);

            // If income is not found, return 404
            if (existingIncome == null)
            {
                return NotFound();
            }

            // Update the fields manually
            existingIncome.Amount = updatedIncome.Amount;
            existingIncome.Source = updatedIncome.Source;
            existingIncome.CategoryId = updatedIncome.CategoryId;
            existingIncome.Date = updatedIncome.Date;

            // Save changes to the database
            _context.SaveChanges();

            return NoContent(); // Return 204 No Content
        }

        // Delete an income
        [HttpDelete("{id:int}")]
        public IActionResult DeleteIncome(int id)
        {
            var income = _context.Incomes.Find(id);
            if (income == null)
                return NotFound();

            _context.Incomes.Remove(income);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
