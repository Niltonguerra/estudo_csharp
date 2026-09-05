namespace ProductsApi.Modules.Products.Domain.Events;

public record ProductCreatedEvent(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    DateTime CreatedAt
);