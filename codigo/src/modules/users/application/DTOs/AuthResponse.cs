namespace ProductsApi.Modules.Users.Application.DTOs;

public record AuthResponse(
    string Token,
    string Name,
    string Email,
    string Role
);