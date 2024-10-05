// Controllers/CategoryController.cs
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Get all categories
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return Ok(_categoryRepository.GetAllCategories());
        }

        // Get category by ID
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        // Create a new category
        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            _categoryRepository.AddCategory(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // Update category
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            var existingCategory = _categoryRepository.GetCategoryById(id);
            if (existingCategory == null) return NotFound();

            _categoryRepository.UpdateCategory(category);
            return NoContent();
        }

        // Delete category
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null) return NotFound();

            _categoryRepository.DeleteCategory(id);
            return NoContent();
        }
    }
}
