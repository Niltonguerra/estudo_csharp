namespace ProductsApi.Modules.Users.Application.DTOs;

public record UserResponse(
    int Id,
    string Name,
    string Email,
    string Role,
    DateTime CreatedAt
);