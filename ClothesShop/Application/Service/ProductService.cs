using Application.DTOs;
using Domain.Models;
using Infrastructure.Context;
using Domain.Models.Http; // Asegúrate de que esta referencia esté correctamente configurada

namespace Application.Service
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = _context.Products.ToList();
                var productDtos = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Size = p.Size,
                    Color = p.Color,
                    Price = p.Price,
                    Description = p.Description
                });

                return new ApiResponse<IEnumerable<ProductDto>>(productDtos); // Respuesta exitosa
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ProductDto>>(null, false, "Error fetching products: " + ex.Message, 500); // Error
            }
        }

        public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(long id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new ApiResponse<ProductDto>(null, false, "Product not found.", 404); // Producto no encontrado
                }

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Size = product.Size,
                    Color = product.Color,
                    Price = product.Price,
                    Description = product.Description
                };

                return new ApiResponse<ProductDto>(productDto); // Respuesta exitosa
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>(null, false, $"Error fetching product with ID: {id} - {ex.Message}", 500); // Error
            }
        }

        public async Task<ApiResponse<ProductDto>> AddProductAsync(ProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    Size = productDto.Size,
                    Name = productDto.Name,
                    Color = productDto.Color,
                    Price = productDto.Price,
                    Description = productDto.Description
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                var addedProductDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Size = product.Size,
                    Color = product.Color,
                    Price = product.Price,
                    Description = product.Description
                };

                return new ApiResponse<ProductDto>(addedProductDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>(null, false, "Error adding product: " + ex.Message, 500); 
            }
        }

        public async Task<ApiResponse<ProductDto>> UpdateProductAsync(long id, ProductDto productDto)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new ApiResponse<ProductDto>(null, false, "Product not found.", 404); 
                }

                product.Size = productDto.Size;
                product.Name = productDto.Name;
                product.Color = productDto.Color;
                product.Price = productDto.Price;
                product.Description = productDto.Description;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                var updatedProductDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Size = product.Size,
                    Color = product.Color,
                    Price = product.Price,
                    Description = product.Description
                };

                return new ApiResponse<ProductDto>(updatedProductDto); // Respuesta exitosa
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>(null, false, $"Error updating product with ID {id}: {ex.Message}", 500); // Error
            }
        }

        public async Task<ApiResponse<bool>> DeleteProductAsync(long id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                var success = _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true); 
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, false, $"Error deleting product with ID {id}: {ex.Message}", 500); // Error
            }
        }
    }
}
