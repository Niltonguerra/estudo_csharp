using ProductsApi.Modules.Products.Domain.Entities;

namespace ProductsApi.Modules.Products.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    void Update(Product product);
    void Remove(Product product);
    Task SaveChangesAsync();
}