using ProductsApi.Modules.Products.Application.DTOs;
using ProductsApi.Modules.Products.Domain.Entities;
using ProductsApi.Modules.Products.Domain.Interfaces;

namespace ProductsApi.Modules.Products.Application.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var products = await repository.GetAllAsync();
        return products.Select(p => new ProductResponse(
            p.Id, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt
        ));
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return null;

        return new ProductResponse(
            product.Id, product.Name, product.Description, product.Price, product.Stock, product.CreatedAt
        );
    }

    public async Task<ProductResponse> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        await repository.AddAsync(product);
        await repository.SaveChangesAsync();

        return new ProductResponse(
            product.Id, product.Name, product.Description, product.Price, product.Stock, product.CreatedAt
        );
    }

    public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return null;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;

        repository.Update(product);
        await repository.SaveChangesAsync();

        return new ProductResponse(
            product.Id, product.Name, product.Description, product.Price, product.Stock, product.CreatedAt
        );
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return false;

        repository.Remove(product);
        await repository.SaveChangesAsync();

        return true;
    }
}