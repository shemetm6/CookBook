using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class IngredientRepositoryExtension
{
    public static IServiceCollection AddIngredientRepository(this IServiceCollection services)
        => services.AddSingleton<IIngredientRepository, IngredientRepository>();
}
