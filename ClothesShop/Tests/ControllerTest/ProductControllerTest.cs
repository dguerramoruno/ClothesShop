using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Service;
using ProductApi.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockService = new Mock<IProductService>();
        _controller = new ProductsController(_mockService.Object);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOk_WithListOfProducts()
    {
        // Arrange
        var products = new List<ProductDto>
        {
            new ProductDto { Id = 1, Size = "M", Color = "Red", Price = 19.99, Description = "Test 1" },
            new ProductDto { Id = 2, Size = "L", Color = "Blue", Price = 29.99, Description = "Test 2" }
        };

        _mockService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProducts = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnOk_WhenProductExists()
    {
        // Arrange
        var product = new ProductDto { Id = 1, Size = "M", Color = "Red", Price = 19.99, Description = "Test Product" };
        _mockService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetProductById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(1, returnedProduct.Id);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturn200StatusCode()
    {
        // Arrange
        var newProduct = new ProductDto { Size = "M", Color = "Red", Price = 19.99, Description = "New Product" };
        _mockService.Setup(service => service.AddProductAsync(newProduct)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateProduct(newProduct);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturn200StatusCode_WhenProductExists()
    {
        // Arrange
        var existingProduct = new ProductDto { Id = 1, Size = "M", Color = "Red", Price = 19.99, Description = "Updated Product" };
        _mockService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
        _mockService.Setup(service => service.UpdateProductAsync(1, existingProduct)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateProduct(1, existingProduct);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturn200StatusCode_WhenProductExists()
    {
        // Arrange
        _mockService.Setup(service => service.DeleteProductAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(200, statusCodeResult.StatusCode);
    }
}
