using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class UserServiceExtension
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
        => services.AddScoped<IUserService, UserService>();
}
