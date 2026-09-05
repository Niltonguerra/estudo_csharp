using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Modules.Users.Application.DTOs;
using ProductsApi.Modules.Users.Application.Services;
using Microsoft.AspNetCore.RateLimiting;

namespace ProductsApi.Modules.Users;

[ApiController]
[Route("users")]
public class UsersController(IAuthService authService) : ControllerBase
{

    [HttpPost("register")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var response = await authService.RegisterAsync(dto);
        return CreatedAtAction(nameof(GetMe), response);
    }

    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var response = await authService.LoginAsync(dto);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await authService.GetByIdAsync(userId);
        if (user is null)
            return NotFound();

        return Ok(user);
    }
}