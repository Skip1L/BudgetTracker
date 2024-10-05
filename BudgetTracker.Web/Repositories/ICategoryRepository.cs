using BudgetTracker.Web.Models;

namespace BudgetTracker.Web.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAllCategories();
    Category GetCategoryById(int id);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(int id);
    bool CategoryExists(int id);
}