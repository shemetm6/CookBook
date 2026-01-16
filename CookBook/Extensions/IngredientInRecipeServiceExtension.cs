using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class IngredientInRecipeServiceExtension
{
    public static IServiceCollection AddIngredientInRecipeService(this IServiceCollection services)
        => services.AddSingleton<IIngredientInRecipeService, IngredientInRecipeService>();
}
