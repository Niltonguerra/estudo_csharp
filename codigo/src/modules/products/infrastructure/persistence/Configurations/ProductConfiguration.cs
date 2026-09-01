using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsApi.Modules.Products.Domain.Entities;

namespace ProductsApi.Modules.Products.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(p => p.Description)
               .HasMaxLength(1000);

        builder.Property(p => p.Price)
               .IsRequired();

        builder.Property(p => p.Stock)
               .IsRequired();

        builder.Property(p => p.CreatedAt)
               .IsRequired();
    }
}