using Microsoft.EntityFrameworkCore;
using ProductsApi.Infrastructure.Persistence;
using ProductsApi.Modules.Users.Domain.Entities;
using ProductsApi.Modules.Users.Domain.Interfaces;

namespace ProductsApi.Modules.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly DbSet<User> _users = context.Set<User>();

    public async Task<User?> GetByIdAsync(int id)
        => await _users.FindAsync(id);

    public async Task<User?> GetByEmailAsync(string email)
        => await _users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user)
        => await _users.AddAsync(user);

    public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
}