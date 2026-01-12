using CookBook.Enums;
using CookBook.Models;
using CookBook.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    public RecipeController(IRecipeService recipeRepository)
        => _recipeService = recipeRepository;

    [HttpPost]
    public ActionResult<int> AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        [FromBody] List<IngredientInRecipe> ingredients,
        string descritption)
    {
        var id = _recipeService.AddRecipe(title.Trim(), cookTime, timeUnit, descritption.Trim(), ingredients);

        return Ok(id);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateRecipe(
        int id,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        [FromBody] List<IngredientInRecipe> ingredients,
        string descritption)
    {
        _recipeService.UpdateRecipe(id, title.Trim(), cookTime, timeUnit, descritption.Trim(), ingredients);

        return NoContent();
    }

    [HttpPut("{id}/rating")]
    public ActionResult RateRecipe(int id, Raiting raiting)
    {
        _recipeService.RateRecipe(id, raiting);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(int id)
    {
        _recipeService.DeleteRecipe(id);

        return NoContent();
    }

    [HttpGet]
    public ActionResult<List<Recipe>> GetRecipes()
        => Ok(_recipeService.GetRecipes());

    [HttpGet("{id}")]
    public ActionResult<Recipe> GetRecipe(int id)
        => Ok(_recipeService.GetRecipe(id));
}
