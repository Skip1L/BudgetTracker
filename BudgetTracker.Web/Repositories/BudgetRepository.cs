// Repositories/BudgetRepository.cs
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetTracker.Web.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly BudgetTrackerContext _context;

        public BudgetRepository(BudgetTrackerContext context)
        {
            _context = context;
        }

        public Budget GetBudgetById(int id)
        {
            return _context.Budgets.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Budget> GetAllBudgets()
        {
            return _context.Budgets.ToList();
        }

        public void AddBudget(Budget budget)
        {
            _context.Budgets.Add(budget);
            _context.SaveChanges();
        }

        public void UpdateBudget(Budget budget)
        {
            // Check if the entity is already tracked and detach it
            var trackedEntity = _context.Budgets.Local.FirstOrDefault(b => b.Id == budget.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            // Update the budget
            _context.Budgets.Update(budget);
            _context.SaveChanges();
        }

        public void DeleteBudget(int id)
        {
            var budget = _context.Budgets.Find(id);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
                _context.SaveChanges();
            }
        }

        public bool BudgetExists(int id)
        {
            return _context.Budgets.Any(b => b.Id == id);
        }
    }
}