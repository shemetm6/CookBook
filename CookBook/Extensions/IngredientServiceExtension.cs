using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class IngredientServiceExtension
{
    public static IServiceCollection AddIngredientService(this IServiceCollection services)
        => services.AddSingleton<IIngredientService, IngredientService>();
}