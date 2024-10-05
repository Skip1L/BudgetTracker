// Repositories/IncomeRepository.cs
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetTracker.Web.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly BudgetTrackerContext _context;

        public IncomeRepository(BudgetTrackerContext context)
        {
            _context = context;
        }

        // Get all incomes for a specific budget
        public IEnumerable<Income> GetAllIncomes(int budgetId)
        {
            return _context.Incomes
                .Where(i => i.BudgetId == budgetId)
                .Include(i => i.Category)
                .ToList();
        }

        // Get an income by ID
        public Income GetIncomeById(int id)
        {
            return _context.Incomes
                .Include(i => i.Category)
                .FirstOrDefault(i => i.Id == id);
        }

        // Add a new income
        public void AddIncome(Income income)
        {
            _context.Incomes.Add(income);
            _context.SaveChanges();
        }

        // Update an existing income
        public void UpdateIncome(Income income)
        {
            _context.Incomes.Update(income);
            _context.SaveChanges();
        }

        // Delete an income by ID
        public void DeleteIncome(int id)
        {
            var income = _context.Incomes.Find(id);
            if (income != null)
            {
                _context.Incomes.Remove(income);
                _context.SaveChanges();
            }
        }

        // Check if an income exists by ID
        public bool IncomeExists(int id)
        {
            return _context.Incomes.Any(i => i.Id == id);
        }
    }
}