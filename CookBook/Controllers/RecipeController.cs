using CookBook.Enums;
using CookBook.Models;
using CookBook.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController] // Зачем это здесь? Что конкретно делают атрибуты? Это какая то дополнительная настройка для типов?
[Route("api/[controller]")] // Это мы так route указываем? Как в первом параметре у методов MapXXX?
public class RecipeController : ControllerBase // Алексей в лекции наследовался от ControllerBase, а не Controller. В чем разница?
{
    private static readonly List<Recipe> _recipes = new List<Recipe>();

    [HttpPost]
    public ActionResult<int> AddRecipe(string title, int cookTimeInMinutes, string ingredients, string descritption)
    {
        var Id = GenerateRecipeIdAndThrowIfDuplicate();
        _recipes.Add(new Recipe()
        {
            Id = Id,
            Title = title.Trim(),
            CookTimeInMinutes = cookTimeInMinutes,
            Ingredients = TryParseIngredientsAndThrowIfNotAllowed(ingredients),
            Descritption = descritption.Trim()
        });

        return Ok(Id);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateRecipe(int id,
        string? title,
        int? cookTimeInMinutes,
        string? ingredients,
        string? descritption)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);

        recipe.Title = title?.Trim() ?? recipe.Title;
        recipe.CookTimeInMinutes = cookTimeInMinutes ?? recipe.CookTimeInMinutes;
        recipe.Ingredients = TryParseIngredientsAndThrowIfNotAllowed(ingredients) ?? recipe.Ingredients; // Разберись как убрать подчеркивание
        recipe.Descritption = descritption?.Trim() ?? recipe.Descritption;

        return NoContent();
    }

    [HttpPut("{id}")]
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

    private static int GenerateRecipeIdAndThrowIfDuplicate()
    {
        var id = _recipes.Count;

        if (_recipes.Any(r => r.Id == id))
            throw new RecipeIdDuplicateException(id);

        return id;
    }

    static List<string> TryParseIngredientsAndThrowIfNotAllowed(string ingredientsInput)
    {
        var ingredients = ingredientsInput
            .Split(',')
            .Select(i => i.Trim())
            .ToList();

        var allowedList = Enum.GetNames<AllowedIngredients>().Select(i => i.ToLower());

        var invalidIngredients = ingredients
            .Where(i => !allowedList.Contains(i.ToLower()))
            .ToList();

        if (invalidIngredients.Any())
            throw new IngredientNotAllowedException(invalidIngredients);

        return ingredients;
    }
}
