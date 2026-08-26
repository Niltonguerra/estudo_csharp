using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.DTOs;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext db, TokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var exists = await db.Users.AnyAsync(u => u.Username == dto.Username);
        if (exists)
            return Conflict(new { message = "Username already taken." });

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User"
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Created($"/api/auth/{user.Id}", new { user.Id, user.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials." });

        var token = tokenService.GenerateToken(user);
        return Ok(new TokenResponseDto(token));
    }
}
