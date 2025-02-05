using Application.DTOs;
using Domain.Models.Http;

namespace Application.Service
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<ApiResponse<ProductDto>> GetProductByIdAsync(long id);
        Task<ApiResponse<ProductDto>> AddProductAsync(ProductDto productDto);
        Task<ApiResponse<ProductDto>> UpdateProductAsync(long id, ProductDto productDto);
        Task<ApiResponse<bool>> DeleteProductAsync(long id);
    }
}