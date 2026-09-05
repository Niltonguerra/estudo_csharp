using ProductsApi.Modules.Users.Domain.Entities;

namespace ProductsApi.Modules.Users.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}