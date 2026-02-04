using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class AuthServiceExtension
{
    public static IServiceCollection AddAuthService(this IServiceCollection services)
        => services.AddScoped<IAuthService, AuthService>();
}
