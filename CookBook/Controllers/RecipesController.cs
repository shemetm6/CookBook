using CookBook.Abstractions;
using Microsoft.AspNetCore.Mvc;
using CookBook.Contracts;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    public RecipesController(IRecipeService recipeRepository)
        => _recipeService = recipeRepository;

    [HttpPost]
    public ActionResult<int> AddRecipe(CreateRecipeDto dto)
    {
        var id = _recipeService.AddRecipe(dto);

        return Ok(id);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateRecipe(int id, UpdateRecipeDto dto)
    {
        _recipeService.UpdateRecipe(id, dto);

        return NoContent();
    }
    /* Work in progress
    [HttpPut("{id}/raiting")]
    public ActionResult RateRecipe(int id, RateRecipeDto dto)
    {
        _recipeService.RateRecipe(id, dto);

        return NoContent();
    }
    */
    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(int id)
    {
        _recipeService.DeleteRecipe(id);

        return NoContent();
    }

    [HttpGet]
    public ActionResult<ListOfRecipes> GetRecipes()
        => Ok(_recipeService.GetRecipes());

    [HttpGet("{id}")]
    public ActionResult<RecipeVm> GetRecipe(int id)
        => Ok(_recipeService.GetRecipe(id));
}
