using Microsoft.EntityFrameworkCore;
using ProductsApi.Modules.Products.Domain.Entities;
using ProductsApi.Modules.Users.Domain.Entities;

namespace ProductsApi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}