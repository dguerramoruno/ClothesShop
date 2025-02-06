using Domain.DTOs;
using Domain.Models;
using Infrastructure.Context;
using Domain.Models.Http;

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

                return new ApiResponse<IEnumerable<ProductDto>>(productDtos); 
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ProductDto>>(null, false, "Error fetching products: " + ex.Message, 500); 
            }
        }

        public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(long id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new ApiResponse<ProductDto>(null, false, "Product not found.", 404); 
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

                return new ApiResponse<ProductDto>(productDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>(null, false, $"Error fetching product with ID: {id} - {ex.Message}", 500); 
            }
        }

        public async Task<ApiResponse<ProductDto>> AddProductAsync(ProductDto productDto)
        {
            try
            {
                var existingProduct = _context.Products.Where(p => p.Name.Equals(productDto.Name)).FirstOrDefault();
                if (existingProduct != null) {
                    return new ApiResponse<ProductDto>(null, false, "Product al ready exists", HttpStatusCodes.InternalServerError);
                }
                if (productDto.Price <= 0)
                {
                    return new ApiResponse<ProductDto>(null, false, "Price must be bigger than 0", HttpStatusCodes.InternalServerError);
                }
                var product = new Product
                {
                    Size = productDto.Size,
                    Name = productDto.Name,
                    Color = productDto.Color,
                    Price = productDto.Price,
                    Description = productDto.Description
                };

                await _context.Products.AddAsync(product);
                var result = await _context.SaveChangesAsync();
                if(result != 0) { 

                    return new ApiResponse<ProductDto>(productDto);
                }

                return new ApiResponse<ProductDto>(null, false, "Product wasn't inserted", HttpStatusCodes.InternalServerError);
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
                var existingNameProduct = _context.Products.Where(p => p.Name.Equals(productDto.Name) & p.Id != id).FirstOrDefault();
                if (existingNameProduct != null) {
                    return new ApiResponse<ProductDto>(null, false, "This product name already exists.", 500);
                }
                if (productDto.Price <= 0)
                {
                    return new ApiResponse<ProductDto>(null, false, "Price must be bigger than 0", HttpStatusCodes.InternalServerError);
                }
                product.Size = productDto.Size;
                product.Name = productDto.Name;
                product.Color = productDto.Color;
                product.Price = productDto.Price;
                product.Description = productDto.Description;

                _context.Products.Update(product);
                var response = await _context.SaveChangesAsync();

                if (response != 0)
                {
                    return new ApiResponse<ProductDto>(productDto);
                }
                return new ApiResponse<ProductDto>(null, false, "Error updating product at DB",HttpStatusCodes.InternalServerError);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>(null, false, $"Error updating product with ID {id}: {ex.Message}", 500); 
            }
        }

        public async Task<ApiResponse<bool>> DeleteProductAsync(long id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) 
                {
                    return new ApiResponse<bool>(false,false,"This product was not found",HttpStatusCodes.InternalServerError);
                }
                var success = _context.Products.Remove(product);
                var response = await _context.SaveChangesAsync();
                if(response != 0) { 
                    return new ApiResponse<bool>(true);
                }
                return new ApiResponse<bool>(false, false, "Error deleting this product in Database", HttpStatusCodes.InternalServerError);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, false, $"Error deleting product with ID {id}: {ex.Message}", 500); 
            }
        }
    }
}
