using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class RecipeRepositoryExtension
{
    public static IServiceCollection AddRecipeRepository(this IServiceCollection services)
        => services.AddSingleton<IRecipeRepository, RecipeRepository>();
}
