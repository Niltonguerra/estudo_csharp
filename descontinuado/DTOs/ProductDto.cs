namespace ProductsApi.DTOs;

public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    int Stock
);

public record UpdateProductDto(
    string? Name,
    string? Description,
    decimal? Price,
    int? Stock
);

public record LoginDto(string Username, string Password);

public record RegisterDto(string Username, string Password);

public record TokenResponseDto(string Token);
