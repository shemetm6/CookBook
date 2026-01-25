using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Models;

namespace CookBook.Services;

public class RecipeRepository : IRecipeRepository
{
    private readonly List<Recipe> _recipes = [];
    private readonly ITimeConverter _timeConverter;

    public RecipeRepository(ITimeConverter timeConverter) => _timeConverter = timeConverter;

    public int AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string descritption
    )
    {
        var recipeId = _recipes.Count;

        var recipe = new Recipe
        {
            Id = recipeId,
            Title = title,
            CookTime = _timeConverter.Convert(cookTime, timeUnit),
            Description = descritption,
        };
        _recipes.Add(recipe);

        return recipeId;
    }

    public void UpdateRecipe(
        int recipeId,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string descritption
    )
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(recipeId);

        recipe.Title = title;
        recipe.CookTime = _timeConverter.Convert(cookTime, timeUnit);
        recipe.Description = descritption;
    }

    public void RateRecipe(int id, Rating rating)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);

        recipe.Ratings.Add((int)rating);
    }

    public void DeleteRecipe(int id)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);

        _recipes.Remove(recipe);
    }

    public IReadOnlyList<Recipe> GetRecipes() => _recipes;

    public Recipe GetRecipe(int id) => TryGetRecipeAndThrowIfNotFound(id);

    private Recipe TryGetRecipeAndThrowIfNotFound(int id)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id == id);

        return recipe ?? throw new RecipeNotFoundException(id);
    }
}
