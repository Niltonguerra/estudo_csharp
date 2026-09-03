using ProductsApi.Modules.Products.Application.Services;
using ProductsApi.Modules.Products.Domain.Interfaces;
using ProductsApi.Modules.Products.Infrastructure.Persistence.Repositories;

namespace ProductsApi.Modules.Products;

public static class ProductsModule
{
    public static IServiceCollection AddProductsModule(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}