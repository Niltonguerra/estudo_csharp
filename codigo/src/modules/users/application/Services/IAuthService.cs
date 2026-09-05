using ProductsApi.Modules.Users.Application.DTOs;

namespace ProductsApi.Modules.Users.Application.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterDto dto);
    Task<AuthResponse> LoginAsync(LoginDto dto);
    Task<UserResponse?> GetByIdAsync(int id);
}