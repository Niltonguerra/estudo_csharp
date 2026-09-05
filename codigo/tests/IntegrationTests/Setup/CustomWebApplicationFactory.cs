using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductsApi.Infrastructure.Persistence;

namespace IntegrationTests.Setup;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove o AppDbContext registrado (PostgreSQL)
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Adiciona o banco em memória
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase("TestDb"));
        });

        // Desabilita o Serilog nos testes
        builder.UseSetting("Logging:LogLevel:Default", "Warning");
    }
}