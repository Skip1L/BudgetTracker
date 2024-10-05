using BudgetTracker.Web.Controllers;
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using BudgetTracker.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BudgetTracker.Web.Test;

/*public class BudgetControllerTests
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
}*/

public class BudgetControllerTests
{
    private readonly Mock<IBudgetRepository> _mockBudgetRepo;
    private readonly BudgetController _controller;

    public BudgetControllerTests()
    {
        _mockBudgetRepo = new Mock<IBudgetRepository>();
        _controller = new BudgetController(_mockBudgetRepo.Object);
    }

    [Fact]
    public void GetBudgets_ReturnsAllBudgets()
    {
        // Arrange
        _mockBudgetRepo.Setup(repo => repo.GetAllBudgets())
            .Returns(new List<Budget> { new Budget { Id = 1, Name = "Test Budget" } });

        // Act
        var result = _controller.GetBudgets();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var budgets = Assert.IsType<List<Budget>>(okResult.Value);
        Assert.Single(budgets);
    }

    [Fact]
    public void GetBudget_ReturnsBudgetById()
    {
        // Arrange
        var budget = new Budget { Id = 1, Name = "Test Budget" };
        _mockBudgetRepo.Setup(repo => repo.GetBudgetById(1)).Returns(budget);

        // Act
        var result = _controller.GetBudget(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Budget>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public void CreateBudget_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var newBudget = new Budget { Id = 1, Name = "New Budget" };

        // Act
        var result = _controller.CreateBudget(newBudget);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Budget>(createdAtActionResult.Value);
        Assert.Equal("New Budget", returnValue.Name);
    }

    [Fact]
    public void UpdateBudget_ReturnsNoContent()
    {
        // Arrange
        var existingBudget = new Budget { Id = 1, Name = "Updated Budget" };
        _mockBudgetRepo.Setup(repo => repo.GetBudgetById(1)).Returns(existingBudget);

        // Act
        var result = _controller.UpdateBudget(1, existingBudget);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteBudget_ReturnsNoContent()
    {
        // Arrange
        var existingBudget = new Budget { Id = 1, Name = "Test Budget" };
        _mockBudgetRepo.Setup(repo => repo.GetBudgetById(1)).Returns(existingBudget);

        // Act
        var result = _controller.DeleteBudget(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}