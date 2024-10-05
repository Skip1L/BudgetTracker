using BudgetTracker.Web.Controllers;
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Web.Test;

public class BudgetControllerTests
{
    // Method to create a new in-memory database instance with test data
    private BudgetTrackerContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<BudgetTrackerContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // Use a unique name for each test
            .Options;

        var context = new BudgetTrackerContext(options);
        return context;
    }

    [Fact]
    public void GetBudgets_ReturnsAllBudgets()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Budgets.Add(new Budget { Id = 1, Name = "Test Budget", TotalAmount = 1000 });
        context.SaveChanges();

        var controller = new BudgetController(context);

        // Act
        var result = controller.GetBudgets();

        // Assert
        Assert.Single(result.Value!);
    }

    [Fact]
    public void CreateBudget_AddsNewBudget()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new BudgetController(context);
        var newBudget = new Budget { Name = "New Budget", TotalAmount = 500 };

        // Act
        controller.CreateBudget(newBudget);

        // Assert
        Assert.Equal(1, context.Budgets.Count());
    }

    [Fact]
    public void DeleteBudget_RemovesBudget()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Budgets.Add(new Budget { Id = 1, Name = "Test Budget", TotalAmount = 1000 });
        context.SaveChanges();

        var controller = new BudgetController(context);

        // Act
        controller.DeleteBudget(1);

        // Assert
        Assert.Empty(context.Budgets);
    }
}