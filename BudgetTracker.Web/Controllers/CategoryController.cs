// Controllers/CategoryController.cs
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
    public class CategoryController : ControllerBase
    {
        private readonly BudgetTrackerContext _context;

        public CategoryController(BudgetTrackerContext context)
        {
            _context = context;
        }

        // Get all categories
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return _context.Categories.ToList();
        }

        // Get category by ID
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        // Create a new category
        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            if (category == null)
                return BadRequest();

            _context.Categories.Add(category);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // Update an existing category
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            // Find the existing category by ID
            var existingCategory = _context.Categories.Find(id);

            // If category is not found, return 404
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Update the fields manually
            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;

            // Save changes to the database
            _context.SaveChanges();

            return NoContent(); // Return 204 No Content
        }

        // Delete category
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
