using Microsoft.EntityFrameworkCore;
using ProductsApi.Infrastructure.Persistence;
using ProductsApi.Modules.Products.Domain.Entities;
using ProductsApi.Modules.Products.Domain.Interfaces;

namespace ProductsApi.Modules.Products.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
	private readonly DbSet<Product> _products = context.Set<Product>();

	public async Task<Product?> GetByIdAsync(int id)
		=> await _products.FindAsync(id);

	public async Task<IEnumerable<Product>> GetAllAsync()
		=> await _products.ToListAsync();

	public async Task AddAsync(Product product)
		=> await _products.AddAsync(product);

	public void Update(Product product)
		=> _products.Update(product);

	public void Remove(Product product)
		=> _products.Remove(product);

	public async Task SaveChangesAsync()
		=> await context.SaveChangesAsync();
}