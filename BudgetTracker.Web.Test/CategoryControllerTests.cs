﻿using BudgetTracker.Web.Controllers;
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Models;
using BudgetTracker.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BudgetTracker.Web.Test;

/*public class CategoryControllerTests
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
    public void GetCategories_ReturnsAllCategories()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Categories.Add(new Category { Id = 1, Name = "Food", Description = "Food expenses" });
        context.Categories.Add(new Category { Id = 2, Name = "Transport", Description = "Transport expenses" });
        context.SaveChanges();

        var controller = new CategoryController(context);

        // Act
        var result = controller.GetCategories();

        // Assert
        Assert.Equal(2, result.Value!.Count());
    }

    [Fact]
    public void GetCategory_ReturnsCategoryById()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Categories.Add(new Category { Id = 1, Name = "Food", Description = "Food expenses" });
        context.SaveChanges();

        var controller = new CategoryController(context);

        // Act
        var result = controller.GetCategory(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Category>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal("Food", returnValue.Name);
    }

    [Fact]
    public void CreateCategory_AddsNewCategory()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new CategoryController(context);
        var newCategory = new Category { Name = "New Category", Description = "Description" };

        // Act
        var result = controller.CreateCategory(newCategory);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdCategory = Assert.IsType<Category>(createdAtActionResult.Value);
        Assert.Equal("New Category", createdCategory.Name);
        Assert.Equal(1, context.Categories.Count());
    }

    [Fact]
    public void UpdateCategory_UpdatesExistingCategory()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Categories.Add(new Category { Id = 1, Name = "Food", Description = "Food expenses" });
        context.SaveChanges();

        var controller = new CategoryController(context);
        var updatedCategory = new Category { Id = 1, Name = "Updated Food", Description = "Updated Description" };

        // Act
        var result = controller.UpdateCategory(1, updatedCategory);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedEntity = context.Categories.Find(1);
        Assert.Equal("Updated Food", updatedEntity.Name);
    }

    [Fact]
    public void DeleteCategory_RemovesCategory()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Categories.Add(new Category { Id = 1, Name = "Food", Description = "Food expenses" });
        context.SaveChanges();

        var controller = new CategoryController(context);

        // Act
        var result = controller.DeleteCategory(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Categories);
    }
}*/

public class CategoryControllerTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepo;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mockCategoryRepo = new Mock<ICategoryRepository>();
        _controller = new CategoryController(_mockCategoryRepo.Object);
    }

    [Fact]
    public void GetCategories_ReturnsAllCategories()
    {
        // Arrange
        _mockCategoryRepo.Setup(repo => repo.GetAllCategories())
            .Returns(new List<Category> { new Category { Id = 1, Name = "Food" } });

        // Act
        var result = _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var categories = Assert.IsType<List<Category>>(okResult.Value);
        Assert.Single(categories);
    }

    [Fact]
    public void GetCategory_ReturnsCategoryById()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Food" };
        _mockCategoryRepo.Setup(repo => repo.GetCategoryById(1)).Returns(category);

        // Act
        var result = _controller.GetCategory(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Category>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public void CreateCategory_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var newCategory = new Category { Id = 1, Name = "Transport" };

        // Act
        var result = _controller.CreateCategory(newCategory);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Category>(createdAtActionResult.Value);
        Assert.Equal("Transport", returnValue.Name);
    }

    [Fact]
    public void UpdateCategory_ReturnsNoContent()
    {
        // Arrange
        var existingCategory = new Category { Id = 1, Name = "Updated Category" };
        _mockCategoryRepo.Setup(repo => repo.GetCategoryById(1)).Returns(existingCategory);

        // Act
        var result = _controller.UpdateCategory(1, existingCategory);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteCategory_ReturnsNoContent()
    {
        // Arrange
        var existingCategory = new Category { Id = 1, Name = "Food" };
        _mockCategoryRepo.Setup(repo => repo.GetCategoryById(1)).Returns(existingCategory);

        // Act
        var result = _controller.DeleteCategory(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}