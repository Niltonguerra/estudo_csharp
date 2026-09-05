using ProductsApi.Modules.Users.Application.Services;
using ProductsApi.Modules.Users.Domain.Interfaces;
using ProductsApi.Modules.Users.Infrastructure.Persistence.Repositories;

namespace ProductsApi.Modules.Users;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}