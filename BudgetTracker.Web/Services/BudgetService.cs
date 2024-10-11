// Services/BudgetService.cs
using BudgetTracker.Web.Models;
using BudgetTracker.Web.Repositories;
using System.Collections.Generic;

namespace BudgetTracker.Web.Services
{
    public class BudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BudgetService(
            IBudgetRepository budgetRepository,
            IExpenseRepository expenseRepository,
            IIncomeRepository incomeRepository,
            ICategoryRepository categoryRepository)
        {
            _budgetRepository = budgetRepository;
            _expenseRepository = expenseRepository;
            _incomeRepository = incomeRepository;
            _categoryRepository = categoryRepository;
        }

        // Get a budget by ID
        public Budget GetBudgetById(int id)
        {
            return _budgetRepository.GetBudgetById(id);
        }

        // Get all expenses for a budget
        public IEnumerable<Expense> GetExpensesForBudget(int budgetId)
        {
            return _expenseRepository.GetAllExpenses(budgetId);
        }

        // Get all expenses for a budget
        public IEnumerable<Budget> GetAllBudget()
        {
            return _budgetRepository.GetAllBudgets();
        }

        // Get all incomes for a budget
        public IEnumerable<Income> GetIncomesForBudget(int budgetId)
        {
            return _incomeRepository.GetAllIncomes(budgetId);
        }

        // Get all categories
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }

        // Check if the total expenses exceed a budget limit (e.g., 90%)
        public bool IsBudgetExceeded(int budgetId, decimal warningThreshold = 0.90m)
        {
            var budget = _budgetRepository.GetBudgetById(budgetId);
            if (budget == null) return false;

            var totalExpenses = _expenseRepository.GetTotalExpensesForBudget(budgetId);
            var totalIncomes = _incomeRepository.GetAllIncomes(budgetId).Sum(i => i.Amount);

            // Check if expenses exceed the threshold of the net budget (budget + incomes)
            return totalExpenses >= (budget.TotalAmount + totalIncomes) * warningThreshold;
        }

        // Add more business logic as needed, like handling complex category and budget analysis
    }
}
