using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Application.DTOs;
using Application.Service;
using Domain.Models;
using Domain.Models.Interfaces;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _productService = new ProductService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Size = "M", Color = "Red", Price = 19.99, Description = "Product 1" },
            new Product { Id = 2, Size = "L", Color = "Blue", Price = 29.99, Description = "Product 2" }
        };

        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("M", result.First().Size);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnCorrectProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Size = "M", Color = "Red", Price = 19.99, Description = "Test Product" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("M", result.Size);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductNotFound()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.GetProductByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddProductAsync_ShouldCallRepositoryOnce()
    {
        // Arrange
        var productDto = new ProductDto { Size = "M", Color = "Red", Price = 19.99, Description = "Test Product" };

        // Act
        await _productService.AddProductAsync(productDto);

        // Assert
        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldModifyExistingProduct()
    {
        // Arrange
        var existingProduct = new Product { Id = 1, Size = "M", Color = "Red", Price = 19.99, Description = "Old Description" };
        var productDto = new ProductDto { Size = "L", Color = "Blue", Price = 25.99, Description = "Updated Description" };

        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduct);

        // Act
        await _productService.UpdateProductAsync(1, productDto);

        // Assert
        _mockRepo.Verify(repo => repo.UpdateAsync(It.Is<Product>(p =>
            p.Size == "L" && p.Color == "Blue" && p.Price == 25.99 && p.Description == "Updated Description")),
            Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldCallRepositoryOnce()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        await _productService.DeleteProductAsync(1);

        // Assert
        _mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
}
