// Models/Income.cs
namespace BudgetTracker.Web.Models
{
    public class Income
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }  // Foreign key to Budget
        public int? CategoryId { get; set; } // Foreign key to Category (optional)
        public decimal Amount { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Budget Budget { get; set; }
        public Category Category { get; set; }
    }
}