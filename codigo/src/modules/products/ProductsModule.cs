using ProductsApi.Modules.Products.Application.Services;
using ProductsApi.Modules.Products.Domain.Interfaces;
using ProductsApi.Modules.Products.Infrastructure.Messaging;
using ProductsApi.Modules.Products.Infrastructure.Persistence.Repositories;

namespace ProductsApi.Modules.Products;

public static class ProductsModule
{
    public static IServiceCollection AddProductsModule(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductEventPublisher, ProductEventPublisher>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return ProductEventPublisher.CreateAsync(config).GetAwaiter().GetResult();
        });

        return services;
    }
}