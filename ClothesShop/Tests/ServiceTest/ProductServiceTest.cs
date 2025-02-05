using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Service;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Domain.Models.Http;

public class ProductServiceTests
{
    private readonly ProductDbContext _dbContext;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        // Configurar una base de datos en memoria
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _dbContext = new ProductDbContext(options);

        // Crear un repositorio mock

        // Instanciar el servicio con el contexto y el mock del repositorio
        _productService = new ProductService(_dbContext);
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsListOfProducts()
    {
        // Arrange
        _dbContext.Products.Add(new Product { Id = 543, Name = "Product1", Size = "M", Color = "Red", Price = 100, Description = "Test" });
        _dbContext.Products.Add(new Product { Id = 234, Name = "Product2", Size = "L", Color = "Blue", Price = 200, Description = "Test" });
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _productService.GetAllProductsAsync();

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Count());
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsProduct_WhenExists()
    {
        // Arrange
        _dbContext.Products.Add(new Product { Id = 864, Name = "Product1", Size = "M", Color = "Red", Price = 100, Description = "Test" });
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _productService.GetProductByIdAsync(864);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Equal("Product1", response.Data.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsNotFound_WhenDoesNotExist()
    {
        // Act
        var response = await _productService.GetProductByIdAsync(99);

        // Assert
        Assert.False(response.Success);
        Assert.Equal(404, response.StatusCode);
        Assert.Equal("Product not found.", response.ErrorMessage);
    }

    [Fact]
    public async Task AddProductAsync_AddsProductSuccessfully()
    {
        // Arrange
        var newProduct = new ProductDto { Name = "New Product", Size = "L", Color = "Green", Price = 150, Description = "Test Desc" };

        // Act
        var response = await _productService.AddProductAsync(newProduct);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Equal("New Product", response.Data.Name);
    }

    [Fact]
    public async Task UpdateProductAsync_UpdatesProductSuccessfully()
    {
        // Arrange
        _dbContext.Products.Add(new Product { Id = 1, Name = "Old Product", Size = "M", Color = "Black", Price = 100, Description = "Old Desc" });
        await _dbContext.SaveChangesAsync();
        var updatedProduct = new ProductDto { Name = "Updated Product", Size = "L", Color = "White", Price = 120, Description = "Updated Desc" };

        // Act
        var response = await _productService.UpdateProductAsync(1, updatedProduct);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Equal("Updated Product", response.Data.Name);
    }

    [Fact]
    public async Task DeleteProductAsync_DeletesProductSuccessfully()
    {
        // Arrange
        _dbContext.Products.Add(new Product { Id = 123, Name = "Product to Delete", Size = "S", Color = "Yellow", Price = 50, Description = "Test" });
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _productService.DeleteProductAsync(123);

        // Assert
        Assert.True(response.Success);
    }

    [Fact]
    public async Task DeleteProductAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Act
        var response = await _productService.DeleteProductAsync(0);

        // Assert
        Assert.False(response.Success);
        Assert.Equal(500, response.StatusCode);
    }
}
