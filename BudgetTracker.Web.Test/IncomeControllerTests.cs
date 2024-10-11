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

public class IncomeControllerTests
{
    private readonly Mock<IIncomeRepository> _mockIncomeRepo;
    private readonly Mock<IHubContext<NotificationHub>> _mockHubContext;
    private readonly Mock<IClientProxy> _mockClients;
    private readonly IncomeController _controller;

    public IncomeControllerTests()
    {
        _mockIncomeRepo = new Mock<IIncomeRepository>();
        _mockHubContext = new Mock<IHubContext<NotificationHub>>();
        _mockClients = new Mock<IClientProxy>();

        _mockHubContext.Setup(hub => hub.Clients.All).Returns(_mockClients.Object);
        _controller = new IncomeController(_mockIncomeRepo.Object, _mockHubContext.Object);
    }

    [Fact]
    public void GetIncomes_ReturnsAllIncomesForBudget()
    {
        // Arrange
        _mockIncomeRepo.Setup(repo => repo.GetAllIncomes(1))
            .Returns(new List<Income> { new Income { Id = 1, BudgetId = 1, Amount = 500 } });

        // Act
        var result = _controller.GetIncomes(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var incomes = Assert.IsType<List<Income>>(okResult.Value);
        Assert.Single(incomes);
    }

    [Fact]
    public void GetIncome_ReturnsIncomeById()
    {
        // Arrange
        var income = new Income { Id = 1, BudgetId = 1, Amount = 500 };
        _mockIncomeRepo.Setup(repo => repo.GetIncomeById(1)).Returns(income);

        // Act
        var result = _controller.GetIncome(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Income>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public void CreateIncome_SendsNotification()
    {
        // Arrange
        var newIncome = new Income { Id = 1, BudgetId = 1, Amount = 600, Source = "Freelance" };

        // Act
        var result = _controller.CreateIncome(newIncome);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Income>(createdAtActionResult.Value);

        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Income 'Freelance' of 600 has been added."),
            default), Times.Once);
    }

    [Fact]
    public void UpdateIncome_ReturnsNoContent()
    {
        // Arrange
        var existingIncome = new Income { Id = 1, BudgetId = 1, Amount = 700, Source = "Freelance" };
        _mockIncomeRepo.Setup(repo => repo.GetIncomeById(1)).Returns(existingIncome);

        // Act
        var result = _controller.UpdateIncome(1, existingIncome);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Income 'Freelance' has been updated."),
            default), Times.Once);
    }

    [Fact]
    public void DeleteIncome_ReturnsNoContent()
    {
        // Arrange
        var existingIncome = new Income { Id = 1, BudgetId = 1, Amount = 500, Source = "Freelance" };
        _mockIncomeRepo.Setup(repo => repo.GetIncomeById(1)).Returns(existingIncome);

        // Act
        var result = _controller.DeleteIncome(1);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify that the notification was sent
        _mockClients.Verify(clients => clients.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o[0].ToString() == "Income 'Freelance' has been deleted."),
            default), Times.Once);
    }
}