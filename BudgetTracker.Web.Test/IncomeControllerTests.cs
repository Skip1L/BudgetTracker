using BudgetTracker.Web.Controllers;
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Web.Test;

public class IncomeControllerTests
{
    private BudgetTrackerContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<BudgetTrackerContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        var context = new BudgetTrackerContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public void GetIncomes_ReturnsIncomesForBudget()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Budgets.Add(new Budget { Id = 1, Name = "Test Budget", TotalAmount = 1000 });
        context.Incomes.Add(new Income { Id = 2, BudgetId = 1, Amount = 300, Source = "Freelance" });
        context.Incomes.Add(new Income { Id = 3, BudgetId = 1, Amount = 500, Source = "Freelance 1" });
        context.SaveChanges();

        var controller = new IncomeController(context);

        // Act
        var result = controller.GetIncomes(1);

        // Assert

        Assert.Equal(2, result.Value!.Count());
    }

    [Fact]
    public void GetIncome_ReturnsIncomeById()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Incomes.Add(new Income { Id = 1, BudgetId = 1, Amount = 500, Source = "Salary" });
        context.SaveChanges();

        var controller = new IncomeController(context);

        // Act
        var result = controller.GetIncome(1);

        // Assert
        Assert.Equal(1, result.Value.Id);
        Assert.Equal(500, result.Value.Amount);
    }

    [Fact]
    public void CreateIncome_AddsNewIncome()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new IncomeController(context);
        var newIncome = new Income { BudgetId = 1, Amount = 600, Source = "Contract" };

        // Act
        controller.CreateIncome(newIncome);

        // Assert
        Assert.Equal(600, context.Incomes.Find(1)!.Amount);
        Assert.Equal(1, context.Incomes.Count());
    }

    [Fact]
    public void UpdateIncome_UpdatesExistingIncome()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Incomes.Add(new Income { Id = 1, BudgetId = 1, CategoryId = 1, Amount = 500, Source = "Salary" });
        context.SaveChanges();

        var controller = new IncomeController(context);
        var updatedIncome = new Income { Id = 1, BudgetId = 1, CategoryId = 1, Amount = 700, Source = "Updated Salary" };

        // Act
        controller.UpdateIncome(1, updatedIncome);

        // Assert
        Assert.Equal(700, context.Incomes.Find(1)!.Amount);
    }

    [Fact]
    public void DeleteIncome_RemovesIncome()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Incomes.Add(new Income { Id = 1, BudgetId = 1, Amount = 500, Source = "Salary" });
        context.SaveChanges();

        var controller = new IncomeController(context);

        // Act
        controller.DeleteIncome(1);

        // Assert
        Assert.Empty(context.Incomes);
    }
}