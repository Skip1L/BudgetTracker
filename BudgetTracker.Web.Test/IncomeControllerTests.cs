using BudgetTracker.Web.Controllers;
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using BudgetTracker.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BudgetTracker.Web.Test;

public class IncomeControllerTests
{
    private readonly Mock<IIncomeRepository> _mockIncomeRepo;
    private readonly IncomeController _controller;

    public IncomeControllerTests()
    {
        _mockIncomeRepo = new Mock<IIncomeRepository>();
        _controller = new IncomeController(_mockIncomeRepo.Object);
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
    public void CreateIncome_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var newIncome = new Income { Id = 1, BudgetId = 1, Amount = 600 };

        // Act
        var result = _controller.CreateIncome(newIncome);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Income>(createdAtActionResult.Value);
        Assert.Equal(600, returnValue.Amount);
    }

    [Fact]
    public void UpdateIncome_ReturnsNoContent()
    {
        // Arrange
        var existingIncome = new Income { Id = 1, BudgetId = 1, Amount = 700 };
        _mockIncomeRepo.Setup(repo => repo.GetIncomeById(1)).Returns(existingIncome);

        // Act
        var result = _controller.UpdateIncome(1, existingIncome);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteIncome_ReturnsNoContent()
    {
        // Arrange
        var existingIncome = new Income { Id = 1, BudgetId = 1, Amount = 500 };
        _mockIncomeRepo.Setup(repo => repo.GetIncomeById(1)).Returns(existingIncome);

        // Act
        var result = _controller.DeleteIncome(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}