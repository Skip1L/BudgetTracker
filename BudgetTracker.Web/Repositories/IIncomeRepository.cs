using BudgetTracker.Web.Models;

namespace BudgetTracker.Web.Repositories;

public interface IIncomeRepository
{
    IEnumerable<Income> GetAllIncomes(int budgetId);
    Income GetIncomeById(int id);
    void AddIncome(Income income);
    void UpdateIncome(Income income);
    void DeleteIncome(int id);
    bool IncomeExists(int id);
}