// Repositories/CategoryRepository.cs
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetTracker.Web.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BudgetTrackerContext _context;

        public CategoryRepository(BudgetTrackerContext context)
        {
            _context = context;
        }

        // Get all categories
        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories
                .Include(c => c.Expenses)
                .Include(c => c.Incomes)
                .ToList();
        }

        // Get a category by ID
        public Category GetCategoryById(int id)
        {
            return _context.Categories
                .Include(c => c.Expenses)
                .Include(c => c.Incomes)
                .FirstOrDefault(c => c.Id == id);
        }

        // Add a new category
        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        // Update an existing category
        public void UpdateCategory(Category category)
        {
            var trackedEntity = _context.Categories.Local.FirstOrDefault(b => b.Id == category.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        // Delete a category by ID
        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        // Check if a category exists by ID
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }
    }
}