using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductApi.Controllers;
using Application.Service;
using Application.DTOs;
using Domain.Models.Http;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductsController(_mockProductService.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOk_WhenProductsExist()
    {
        // Arrange
        var products = new List<ProductDto>
        {
            new ProductDto { Id = 1, Name = "Product1", Size = "M", Color = "Red", Price = 100, Description = "Test" },
            new ProductDto { Id = 2, Name = "Product2", Size = "L", Color = "Blue", Price = 200, Description = "Test" }
        };
        _mockProductService
            .Setup(s => s.GetAllProductsAsync())
            .ReturnsAsync(new ApiResponse<IEnumerable<ProductDto>>(products));

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetProductById_ReturnsProduct_WhenExists()
    {
        // Arrange
        var product = new ProductDto { Id = 1, Name = "Product1", Size = "M", Color = "Red", Price = 100, Description = "Test" };
        _mockProductService
            .Setup(s => s.GetProductByIdAsync(1))
            .ReturnsAsync(new ApiResponse<ProductDto>(product));

        // Act
        var result = await _controller.GetProductById(1);

        // Assert
        var okResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenDoesNotExist()
    {
        // Arrange
        _mockProductService
            .Setup(s => s.GetProductByIdAsync(99))
            .ReturnsAsync(new ApiResponse<ProductDto>(null, false, "Product not found.", 404));

        // Act
        var result = await _controller.GetProductById(99);

        // Assert
        var notFoundResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var productDto = new ProductDto { Name = "New Product", Size = "L", Color = "Green", Price = 150, Description = "Test Desc" };
        var response = new ApiResponse<ProductDto>(productDto, true, null, 201);

        _mockProductService
            .Setup(s => s.AddProductAsync(productDto))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateProduct(productDto);

        // Assert
        var createdResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsUpdatedProduct_WhenExists()
    {
        // Arrange
        var updatedProduct = new ProductDto { Name = "Updated Product", Size = "L", Color = "White", Price = 120, Description = "Updated Desc" };
        var response = new ApiResponse<ProductDto>(updatedProduct, true, null, 200);

        _mockProductService
            .Setup(s => s.UpdateProductAsync(1, updatedProduct))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.UpdateProduct(1, updatedProduct);

        // Assert
        var okResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNotFound_WhenDoesNotExist()
    {
        // Arrange
        _mockProductService
            .Setup(s => s.UpdateProductAsync(99, It.IsAny<ProductDto>()))
            .ReturnsAsync(new ApiResponse<ProductDto>(null, false, "Product not found.", 404));

        // Act
        var result = await _controller.UpdateProduct(99, new ProductDto());

        // Assert
        var notFoundResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsSuccess_WhenDeleted()
    {
        // Arrange
        _mockProductService
            .Setup(s => s.DeleteProductAsync(1))
            .ReturnsAsync(new ApiResponse<bool>(true, true, null, 200));

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        var okResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFound_WhenDoesNotExist()
    {
        // Arrange
        _mockProductService
            .Setup(s => s.DeleteProductAsync(99))
            .ReturnsAsync(new ApiResponse<bool>(false, false, "Product not found.", 404));

        // Act
        var result = await _controller.DeleteProduct(99);

        // Assert
        var notFoundResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}
