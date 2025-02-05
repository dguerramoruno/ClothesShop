using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Domain.Models;
using Application.DTOs;
using Application.Service;

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

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products); 
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = "Error fetching Products.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error Fetching Product.", details = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto product)
        {
            try
            {
                await _productService.AddProductAsync(product);
                return StatusCode(200, product);

            }
            catch (Exception ex) { 
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto updatedProduct)
        {
            try { 
            await _productService.UpdateProductAsync(id, updatedProduct);
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct.Id != id )
                return StatusCode(500, "Error Updating Product");
            
            return StatusCode(200, existingProduct);
            }
            

            catch (Exception ex) 
            {
                 return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
               await _productService.DeleteProductAsync(id);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
