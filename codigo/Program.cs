using Serilog;
using Serilog.AspNetCore;
using ProductsApi.Infrastructure.Extensions;
using ProductsApi.Infrastructure.Middleware;
using ProductsApi.Modules.Products;
using ProductsApi.Modules.Users;

LoggingExtensions.AddLogging();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddRateLimiting();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddProductsModule();
builder.Services.AddUsersModule();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseRateLimiter();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();