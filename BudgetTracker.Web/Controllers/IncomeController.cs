// Controllers/IncomeController.cs
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
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeRepository _incomeRepository;

        public IncomeController(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        // Get all incomes for a budget
        [HttpGet("{budgetId}")]
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
            return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, income);
        }

        // Update income
        [HttpPut("{id:int}")]
        public IActionResult UpdateIncome(int id, [FromBody] Income income)
        {
            var existingIncome = _incomeRepository.GetIncomeById(id);
            if (existingIncome == null) return NotFound();

            _incomeRepository.UpdateIncome(income);
            return NoContent();
        }

        // Delete income
        [HttpDelete("{id:int}")]
        public IActionResult DeleteIncome(int id)
        {
            var income = _incomeRepository.GetIncomeById(id);
            if (income == null) return NotFound();

            _incomeRepository.DeleteIncome(id);
            return NoContent();
        }
    }
}
