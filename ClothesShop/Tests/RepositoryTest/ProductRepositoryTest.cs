using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Infrastructure.Repository;
using Domain.Models;

public class ProductRepositoryTests
{
    private ProductDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        return new ProductDbContext(options);
    }


    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var dbContext = GetDbContext();
        var repository = new ProductRepository(dbContext);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, ((List<Product>)result).Count);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProduct()
    {
        // Arrange
        var dbContext = GetDbContext();
        var repository = new ProductRepository(dbContext);
        var product = new Product { Id = 111, Size = "M", Color = "Red", Price = 19.99, Description = "Test Product" };

        // Act
        await repository.AddAsync(product);
        var result = await repository.GetByIdAsync(111);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("M", result.Size);
        Assert.Equal("Red", result.Color);
    }

    

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectProduct()
    {
        // Arrange
        var dbContext = GetDbContext();
        var repository = new ProductRepository(dbContext);
        var product = new Product { Id = 123, Size = "M", Color = "Red", Price = 19.99, Description = "Test Product" };
        await repository.AddAsync(product);

        // Act
        var result = await repository.GetByIdAsync(123);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.Id);
        Assert.Equal("M", result.Size);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyProduct()
    {
        // Arrange
        var dbContext = GetDbContext();
        var repository = new ProductRepository(dbContext);
        var product = new Product { Id = 453, Size = "M", Color = "Red", Price = 19.99, Description = "Test Product" };
        await repository.AddAsync(product);

        // Act
        product.Size = "L";
        await repository.UpdateAsync(product);
        var updatedProduct = await repository.GetByIdAsync(453);

        // Assert
        Assert.NotNull(updatedProduct);
        Assert.Equal("L", updatedProduct.Size);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProduct()
    {
        // Arrange
        var dbContext = GetDbContext();
        var repository = new ProductRepository(dbContext);
        var product = new Product { Id = 1, Size = "M", Color = "Red", Price = 19.99, Description = "Test Product" };
        await repository.AddAsync(product);

        // Act
        await repository.DeleteAsync(1);
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }
}
