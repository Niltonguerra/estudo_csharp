using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductsApi.Modules.Users.Application.DTOs;
using ProductsApi.Modules.Users.Domain.Entities;
using ProductsApi.Modules.Users.Domain.Interfaces;

namespace ProductsApi.Modules.Users.Application.Services;

public class AuthService(IUserRepository repository, IConfiguration config) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
    {
        var existing = await repository.GetByEmailAsync(dto.Email);
        if (existing is not null)
            throw new ArgumentException("Email já cadastrado");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role ?? "User"
        };

        await repository.AddAsync(user);
        await repository.SaveChangesAsync();

        return new AuthResponse(GenerateToken(user), user.Name, user.Email, user.Role);
    }

    public async Task<AuthResponse> LoginAsync(LoginDto dto)
    {
        var user = await repository.GetByEmailAsync(dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email ou senha inválidos");

        return new AuthResponse(GenerateToken(user), user.Name, user.Email, user.Role);
    }

    public async Task<UserResponse?> GetByIdAsync(int id)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null)
            return null;

        return new UserResponse(user.Id, user.Name, user.Email, user.Role, user.CreatedAt);
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}