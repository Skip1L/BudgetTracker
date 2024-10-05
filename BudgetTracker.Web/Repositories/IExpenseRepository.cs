using BudgetTracker.Web.Models;

namespace BudgetTracker.Web.Repositories
{
    public interface IExpenseRepository
    {
        decimal GetTotalExpensesForBudget(int budgetId);
        IEnumerable<Expense> GetAllExpenses(int budgetId);
        Expense GetExpenseById(int id);
        void AddExpense(Expense expense);
        void UpdateExpense(Expense expense);
        void DeleteExpense(int id);
    }
}
