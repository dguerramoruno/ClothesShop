using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Domain.Models;
using Domain.DTOs;
using Application.Service;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var result = await _productService.GetAllProductsAsync().ConfigureAwait(false);
                return StatusCode(result.StatusCode,result); 
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = "Error fetching Products.", details = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            try
            {
                var result = await _productService.GetProductByIdAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error Fetching Product.", details = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto product)
        {
            try
            {
                var result = await _productService.AddProductAsync(product);
                return StatusCode(result.StatusCode, result);

            }
            catch (Exception ex) { 
                return StatusCode(500, ex);
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto updatedProduct)
        {
            try { 
                var result = await _productService.UpdateProductAsync(id, updatedProduct);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex) 
            {
                 return StatusCode(500, ex);
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
