// Controllers/BudgetController.cs
using BudgetTracker.Web.Repositories;
using BudgetTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BudgetTracker.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetController(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        // Get all budgets
        [HttpGet]
        public ActionResult<IEnumerable<Budget>> GetBudgets()
        {
            return Ok(_budgetRepository.GetAllBudgets());
        }

        // Get budget by ID
        [HttpGet("{id}")]
        public ActionResult<Budget> GetBudget(int id)
        {
            var budget = _budgetRepository.GetBudgetById(id);
            if (budget == null) return NotFound();
            return Ok(budget);
        }

        // Create a new budget
        [HttpPost]
        public IActionResult CreateBudget([FromBody] Budget budget)
        {
            _budgetRepository.AddBudget(budget);
            return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
        }

        // Update budget
        [HttpPut("{id}")]
        public IActionResult UpdateBudget(int id, [FromBody] Budget budget)
        {
            var existingBudget = _budgetRepository.GetBudgetById(id);
            if (existingBudget == null) return NotFound();

            _budgetRepository.UpdateBudget(budget);
            return NoContent();
        }

        // Delete budget
        [HttpDelete("{id}")]
        public IActionResult DeleteBudget(int id)
        {
            var budget = _budgetRepository.GetBudgetById(id);
            if (budget == null) return NotFound();

            _budgetRepository.DeleteBudget(id);
            return NoContent();
        }
    }
}
