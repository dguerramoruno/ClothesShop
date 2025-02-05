using Application.DTOs;

namespace Application.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(long id);
        Task AddProductAsync(ProductDto productDto);
        Task UpdateProductAsync(long id, ProductDto productDto);
        Task DeleteProductAsync(long id);
    }
}
