// Models/Expense.cs
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BudgetTracker.Web.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }  // Foreign key to Budget
        public int? CategoryId { get; set; } // Foreign key to Category (optional)
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ValidateNever]
        public Budget Budget { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
    }
}