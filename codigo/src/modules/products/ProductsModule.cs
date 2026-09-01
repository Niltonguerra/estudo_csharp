namespace ProductsApi.Modules.Products;

public static class ProductsModule
{
    public static IServiceCollection AddProductsModule(this IServiceCollection services)
    {
        // aqui você vai registrar repositories, services e handlers do módulo
        return services;
    }
}