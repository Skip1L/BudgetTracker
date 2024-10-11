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

public class BudgetControllerTests
{
    private readonly Mock<IBudgetRepository> _mockBudgetRepo;
    private readonly Mock<IHubContext<NotificationHub>> _mockHubContext;
    private readonly Mock<IClientProxy> _mockClients;
    private readonly BudgetController _controller;

    public BudgetControllerTests()
    {
        _mockBudgetRepo = new Mock<IBudgetRepository>();
        _mockHubContext = new Mock<IHubContext<NotificationHub>>();
        _mockClients = new Mock<IClientProxy>();

        // Setup Clients to allow SendAsync() to be tested
        _mockHubContext.Setup(hub => hub.Clients.All).Returns(_mockClients.Object);

        _controller = new BudgetController(_mockBudgetRepo.Object, _mockHubContext.Object);
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
    public void CreateBudget_SendsNotification()
    {
        // Arrange
        var newBudget = new Budget { Id = 1, Name = "New Budget" };

        // Act
        var result = _controller.CreateBudget(newBudget);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Budget>(createdAtActionResult.Value);

        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Budget 'New Budget' has been created."),
            default), Times.Once);
    }

    [Fact]
    public void UpdateBudget_SendsNotification()
    {
        // Arrange
        var existingBudget = new Budget { Id = 1, Name = "Updated Budget" };
        _mockBudgetRepo.Setup(repo => repo.GetBudgetById(1)).Returns(existingBudget);

        // Act
        var result = _controller.UpdateBudget(1, existingBudget);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Budget 'Updated Budget' has been updated."),
            default), Times.Once);
    }

    [Fact]
    public void DeleteBudget_SendsNotification()
    {
        // Arrange
        var existingBudget = new Budget { Id = 1, Name = "Test Budget" };
        _mockBudgetRepo.Setup(repo => repo.GetBudgetById(1)).Returns(existingBudget);

        // Act
        var result = _controller.DeleteBudget(1);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Budget 'Test Budget' has been deleted."),
            default), Times.Once);
    }
}