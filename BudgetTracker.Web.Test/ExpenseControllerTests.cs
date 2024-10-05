using BudgetTracker.Web.Controllers;
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Web.Test;

public class ExpenseControllerTests
{
    private BudgetTrackerContext GetContextWithData()
    {
        var options = new DbContextOptionsBuilder<BudgetTrackerContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        var context = new BudgetTrackerContext(options);

        context.Budgets.Add(new Budget { Id = 1, Name = "Test Budget", TotalAmount = 1000 });
        context.Expenses.Add(new Expense { Id = 1, BudgetId = 1, Description = "Test Expense", Amount = 100 });
        context.SaveChanges();

        return context;
    }

    [Fact]
    public void GetExpenses_ReturnsAllExpensesForBudget()
    {
        // Arrange
        var context = GetContextWithData();
        var controller = new ExpenseController(context);

        // Act
        var result = controller.GetExpenses(1);

        // Assert
        Assert.Equal(1, result.Value.Count());
    }

    [Fact]
    public void CreateExpense_AddsNewExpense()
    {
        // Arrange
        var context = GetContextWithData();
        var controller = new ExpenseController(context);
        var newExpense = new Expense { BudgetId = 1, Description = "New Expense", Amount = 50 };

        // Act
        controller.CreateExpense(newExpense);

        // Assert
        Assert.Equal(2, context.Expenses.Count());
    }

    [Fact]
    public void DeleteExpense_RemovesExpense()
    {
        // Arrange
        var context = GetContextWithData();
        var controller = new ExpenseController(context);

        // Act
        controller.DeleteExpense(1);

        // Assert
        Assert.Empty(context.Expenses);
    }
}