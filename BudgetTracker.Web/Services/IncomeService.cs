using BudgetTracker.Web.Models;
using BudgetTracker.Web.Repositories;

namespace BudgetTracker.Web.Services
{
    public class IncomeService
    {
        private readonly IIncomeRepository _incomeRepository;

        public IncomeService(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public IEnumerable<Income> GetAllIncomes(int budgetId) => _incomeRepository.GetAllIncomes(budgetId);

        public Income GetIncomeById(int id) => _incomeRepository.GetIncomeById(id);

        public void AddIncome(Income income) => _incomeRepository.AddIncome(income);

        public void UpdateIncome(Income income) => _incomeRepository.UpdateIncome(income);

        public void DeleteIncome(int id) => _incomeRepository.DeleteIncome(id);

        public bool IncomeExists(int id) => _incomeRepository.IncomeExists(id);
    }
}
