using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class RecipeServiceExtension
{
    public static IServiceCollection AddRecipeService(this IServiceCollection services)
        => services.AddSingleton<IRecipeService, RecipeService>();
}
