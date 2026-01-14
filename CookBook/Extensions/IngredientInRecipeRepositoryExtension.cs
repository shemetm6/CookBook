using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class IngredientInRecipeRepositoryExtension
{
    public static IServiceCollection AddIngredientInRecipeRepository(this IServiceCollection services)
        => services.AddSingleton<IIngredientInRecipeRepository, IngredientInRecipeRepository>();
}
