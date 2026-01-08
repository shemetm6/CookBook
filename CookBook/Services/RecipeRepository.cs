using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Services;

public class RecipeRepository : IRecipeRepository
{
    private readonly List<Recipe> _recipes = new();

    private readonly ITimeConverter _timeConverter;

    public RecipeRepository(ITimeConverter timeConverter)
        => _timeConverter = timeConverter;

    public int AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string ingredients,
        string descritption)
    {
        var recipeId = GenerateIdAndThrowIfDuplicate();

        _recipes.Add(new Recipe()
        {
            Id = recipeId,
            Title = title,
            CookTime = _timeConverter.Convert(cookTime, timeUnit),
            Ingredients = TryParseIngredientsAndThrowIfNotAllowed(ingredients),
            Descritption = descritption
        });

        return recipeId;
    }

    public void UpdateRecipe(
        int id,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string ingredients,
        string descritption)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);

        recipe.Title = title;
        recipe.CookTime = _timeConverter.Convert(cookTime, timeUnit);
        recipe.Ingredients = TryParseIngredientsAndThrowIfNotAllowed(ingredients);
        recipe.Descritption = descritption;
    }

    public void RateRecipe(int id, Raiting raiting)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);

        recipe.Raiting = raiting;
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

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        return recipe;
    }

    private int GenerateIdAndThrowIfDuplicate()
    {
        var id = _recipes.Count;

        if (_recipes.Any(r => r.Id == id))
            throw new RecipeIdDuplicateException(id);

        return id;
    }

    private static List<string> TryParseIngredientsAndThrowIfNotAllowed(string ingredientsInput)
    {
        var ingredients = ingredientsInput
            .Split(',')
            .Select(i => i.Trim())
            .ToList();

        var allowedList = Enum.GetNames<AllowedIngredients>().Select(i => i.ToLower());

        var invalidIngredients = ingredients
            .Where(i => !allowedList.Contains(i.ToLower()))
            .ToList();

        if (invalidIngredients.Count != 0)
            throw new IngredientNotAllowedException(invalidIngredients);

        return ingredients;
    }
}
