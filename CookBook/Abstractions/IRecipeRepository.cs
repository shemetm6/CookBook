using CookBook.Enums;
using CookBook.Models;

namespace CookBook.Abstractions;

public interface IRecipeRepository
{
    public int AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string descritption
    );

    public void UpdateRecipe(
        int recipeId,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string descritption
    );

    public void RateRecipe(int id, Raiting raiting);

    public void DeleteRecipe(int id);

    public IReadOnlyList<Recipe> GetRecipes();

    public Recipe GetRecipe(int id);
}
