using CookBook.Enums;
using CookBook.Models;
using CookBook.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("cookbook/[controller]")]
public class RecipeController : ControllerBase
{
    private static readonly List<Recipe> _recipes = new List<Recipe>();

    [HttpPost]
    public ActionResult<int> AddRecipe(
        string title, 
        int cookTimeInMinutes, 
        string ingredients, 
        string descritption)
    {
        var id = GenerateIdAndThrowIfDuplicate();

        _recipes.Add(new Recipe()
        {
            Id = id,
            Title = title.Trim(),
            CookTimeInMinutes = cookTimeInMinutes,
            Ingredients = TryParseIngredientsAndThrowIfNotAllowed(ingredients),
            Descritption = descritption.Trim()
        });

        return Ok(id);
    }

    // Если я захочу реализовать частичное изменение рецепта, то для этого использовать [HttpPatch("{id}")] ?
    [HttpPut("{id}")]
    public ActionResult UpdateRecipe(
        int id,
        string title,
        int cookTimeInMinutes,
        string ingredients,
        string descritption)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);

        recipe.Title = title.Trim();
        recipe.CookTimeInMinutes = cookTimeInMinutes;
        recipe.Ingredients = TryParseIngredientsAndThrowIfNotAllowed(ingredients);
        recipe.Descritption = descritption.Trim();

        return NoContent();
    }

    [HttpPut("{id}/rating")]
    public ActionResult RateRecipe(int id, Raiting raiting)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);

        recipe.Raiting = raiting;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(int id)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);

        _recipes.Remove(recipe);

        return NoContent();
    }

    [HttpGet]
    public ActionResult<List<Recipe>> GetRecipes() => Ok(_recipes);

    [HttpGet("{id}")]
    public ActionResult<Recipe> GetRecipe(int id) => Ok(TryGetRecipeAndThrowIfNotFound(id));

    private static Recipe TryGetRecipeAndThrowIfNotFound(int id)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id == id);

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        return recipe;
    }

    private static int GenerateIdAndThrowIfDuplicate()
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
