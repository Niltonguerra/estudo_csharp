using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace ProductsApi.Infrastructure.Extensions;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(opt =>
        {
            opt.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                await context.HttpContext.Response.WriteAsync(
                    "Muitas tentativas. Tente novamente mais tarde.", token);
            };

            opt.AddFixedWindowLimiter("auth", o =>
            {
                o.PermitLimit = 5;
                o.Window = TimeSpan.FromMinutes(1);
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 0;
            });
        });

        return services;
    }
}