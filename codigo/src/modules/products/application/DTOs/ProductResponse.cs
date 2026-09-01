namespace ProductsApi.Modules.Products.Application.DTOs;

public record ProductResponse(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    DateTime CreatedAt
);