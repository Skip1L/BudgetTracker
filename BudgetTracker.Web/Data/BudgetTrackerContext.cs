using BudgetTracker.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Web.Data
{
    public class BudgetTrackerContext : DbContext
    {
        public BudgetTrackerContext(DbContextOptions<BudgetTrackerContext> options) : base(options) { }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Budget to Expense relationship (One-to-Many)
            modelBuilder.Entity<Budget>()
                .HasMany(b => b.Expenses)
                .WithOne(e => e.Budget)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Configure Budget to Income relationship (One-to-Many)
            modelBuilder.Entity<Budget>()
                .HasMany(b => b.Incomes)
                .WithOne(i => i.Budget)
                .HasForeignKey(i => i.BudgetId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Configure Category to Expense relationship (One-to-Many)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Expenses)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); // Expenses can exist without a category (optional)

            // Configure Category to Income relationship (One-to-Many)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Incomes)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); // Incomes can exist without a category (optional)

            base.OnModelCreating(modelBuilder);
        }
    }
}
