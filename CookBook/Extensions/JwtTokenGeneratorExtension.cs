using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class JwtTokenGeneratorExtension
{
    public static IServiceCollection AddJwtTokenGenerator(this IServiceCollection services)
        => services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
}
