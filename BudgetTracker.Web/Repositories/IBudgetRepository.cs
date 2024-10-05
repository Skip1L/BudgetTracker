using BudgetTracker.Web.Models;

namespace BudgetTracker.Web.Repositories
{
    public interface IBudgetRepository
    {
        Budget GetBudgetById(int id);
        IEnumerable<Budget> GetAllBudgets();
        void AddBudget(Budget budget);
        void UpdateBudget(Budget budget);
        void DeleteBudget(int id);
        bool BudgetExists(int id);
    }
}
