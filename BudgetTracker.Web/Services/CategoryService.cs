using BudgetTracker.Web.Models;
using BudgetTracker.Web.Repositories;

namespace BudgetTracker.Web.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetAllCategories() => _categoryRepository.GetAllCategories();

        public Category GetCategoryById(int id) => _categoryRepository.GetCategoryById(id);

        public void AddCategory(Category category) => _categoryRepository.AddCategory(category);

        public void UpdateCategory(Category category) => _categoryRepository.UpdateCategory(category);

        public void DeleteCategory(int id) => _categoryRepository.DeleteCategory(id);

        public bool CategoryExists(int id) => _categoryRepository.CategoryExists(id);
    }
}
