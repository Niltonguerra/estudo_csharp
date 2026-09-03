using ProductsApi.Modules.Products.Application.DTOs;

namespace ProductsApi.Modules.Products.Application.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<ProductResponse?> GetByIdAsync(int id);
    Task<ProductResponse> CreateAsync(CreateProductDto dto);
    Task<ProductResponse?> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}