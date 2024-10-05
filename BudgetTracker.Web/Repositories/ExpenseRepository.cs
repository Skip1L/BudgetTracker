// Repositories/ExpenseRepository.cs
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace BudgetTracker.Web.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly BudgetTrackerContext _context;

        public ExpenseRepository(BudgetTrackerContext context)
        {
            _context = context;
        }

        public decimal GetTotalExpensesForBudget(int budgetId)
        {
            return _context.Expenses.Where(e => e.BudgetId == budgetId).Sum(e => e.Amount);
        }

        public IEnumerable<Expense> GetAllExpenses(int budgetId)
        {
            return _context.Expenses.Where(e => e.BudgetId == budgetId).ToList();
        }

        public Expense GetExpenseById(int id)
        {
            return _context.Expenses.FirstOrDefault(e => e.Id == id);
        }

        public void AddExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        public void UpdateExpense(Expense expense)
        {
            _context.Expenses.Update(expense);
            _context.SaveChanges();
        }

        public void DeleteExpense(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _context.SaveChanges();
            }
        }
    }
}