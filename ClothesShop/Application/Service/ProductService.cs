using Application.DTOs;
using Domain.Models.Interfaces;
using Domain.Models;

namespace Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Size = p.Size,
                    Color = p.Color,
                    Price = p.Price,
                    Description = p.Description
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching products", ex);
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(long id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null) return null;

                return new ProductDto
                {
                    Id = product.Id,
                    Size = product.Size,
                    Color = product.Color,
                    Price = product.Price,
                    Description = product.Description
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching product with ID: {id}", ex);
            }
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    Size = productDto.Size,
                    Color = productDto.Color,
                    Price = productDto.Price,
                    Description = productDto.Description
                };

                await _productRepository.AddAsync(product);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding product", ex);
            }
        }

        public async Task UpdateProductAsync(long id, ProductDto productDto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null) return;

                product.Size = productDto.Size;
                product.Color = productDto.Color;
                product.Price = productDto.Price;
                product.Description = productDto.Description;

                await _productRepository.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error Updating product with {id}", ex);
            }
        }

        public async Task DeleteProductAsync(long id)
        {
            try
            {
                await _productRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting product with ID: {id}", ex);
            }
        }
    }
}
