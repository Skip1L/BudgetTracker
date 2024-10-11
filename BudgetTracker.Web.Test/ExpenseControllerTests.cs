using BudgetTracker.Web.Controllers;
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using BudgetTracker.Web.Repositories;
using BudgetTracker.Web.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BudgetTracker.Web.Test;

public class ExpenseControllerTests
{
    private readonly Mock<IExpenseRepository> _mockExpenseRepo;
    private readonly Mock<IHubContext<NotificationHub>> _mockHubContext;
    private readonly Mock<IClientProxy> _mockClients;
    private readonly ExpenseController _controller;

    public ExpenseControllerTests()
    {
        _mockExpenseRepo = new Mock<IExpenseRepository>();
        _mockHubContext = new Mock<IHubContext<NotificationHub>>();
        _mockClients = new Mock<IClientProxy>();

        _mockHubContext.Setup(hub => hub.Clients.All).Returns(_mockClients.Object);
        _controller = new ExpenseController(_mockExpenseRepo.Object, _mockHubContext.Object);
    }

    [Fact]
    public void GetExpenses_ReturnsAllExpensesForBudget()
    {
        // Arrange
        _mockExpenseRepo.Setup(repo => repo.GetAllExpenses(1))
            .Returns(new List<Expense> { new Expense { Id = 1, BudgetId = 1, Amount = 100 } });

        // Act
        var result = _controller.GetExpenses(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var expenses = Assert.IsType<List<Expense>>(okResult.Value);
        Assert.Single(expenses);
    }

    [Fact]
    public void GetExpense_ReturnsExpenseById()
    {
        // Arrange
        var expense = new Expense { Id = 1, BudgetId = 1, Amount = 100 };
        _mockExpenseRepo.Setup(repo => repo.GetExpenseById(1)).Returns(expense);

        // Act
        var result = _controller.GetExpense(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Expense>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public void CreateExpense_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var newExpense = new Expense { Id = 1, BudgetId = 1, Amount = 200, Description = "Test Expense" };

        // Act
        var result = _controller.CreateExpense(newExpense);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Expense>(createdAtActionResult.Value);
        
        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Expense 'Test Expense' has been added with amount 200."),
            default), Times.Once);
    }

    [Fact]
    public void UpdateExpense_ReturnsNoContent()
    {
        // Arrange
        var existingExpense = new Expense { Id = 1, BudgetId = 1, Amount = 300, Description = "Test Expense" };
        _mockExpenseRepo.Setup(repo => repo.GetExpenseById(1)).Returns(existingExpense);

        // Act
        var result = _controller.UpdateExpense(1, existingExpense);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Expense 'Test Expense' has been updated."),
            default), Times.Once);
    }

    [Fact]
    public void DeleteExpense_ReturnsNoContent()
    {
        // Arrange
        var existingExpense = new Expense { Id = 1, BudgetId = 1, Amount = 100, Description = "Test Expense" };
        _mockExpenseRepo.Setup(repo => repo.GetExpenseById(1)).Returns(existingExpense);

        // Act
        var result = _controller.DeleteExpense(1);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Expense 'Test Expense' has been deleted."),
            default), Times.Once);
    }
}