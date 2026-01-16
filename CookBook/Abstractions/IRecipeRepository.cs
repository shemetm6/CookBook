using CookBook.Enums;
using CookBook.Models;

namespace CookBook.Abstractions;

public interface IRecipeRepository
{
    int AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string descritption
    );
    void UpdateRecipe(
        int recipeId,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string descritption
    );
    void RateRecipe(int id, Raiting raiting);
    void DeleteRecipe(int id);
    IReadOnlyList<Recipe> GetRecipes();
    Recipe GetRecipe(int id);
}
