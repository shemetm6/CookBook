using CookBook.Abstractions;
using CookBook.Contracts;
using CookBook.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

public class RecipesController : BaseController
{
    private readonly IRecipeService _recipeService;
    public RecipesController(IRecipeService recipeRepository)
        => _recipeService = recipeRepository;

    [HttpPost]
    public ActionResult<int> AddRecipe(CreateRecipeDto dto)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();
        
        if (userId is null)
            return Unauthorized();

        var recipeId = _recipeService.AddRecipe(dto, userId.Value);

        return Ok(recipeId);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateRecipe(int id, UpdateRecipeDto dto)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        _recipeService.UpdateRecipe(id, dto, userId.Value);

        return NoContent();
    }

    [HttpPut("{id}/raiting")]
    public ActionResult RateRecipe(int id, RateRecipeDto dto)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        _recipeService.RateRecipe(id, dto, userId.Value);

        return NoContent();
    }

    // Не разобрался с policy и убрал соответствующий атрибут.
    // Насколько я понял, при использовании созданной нами политики мне придется иметь параметр userId.
    // А этого хотелось бы избежать т.к. всегда userId это Id авторизованного в данный момент пользователя.
    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(int id)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        _recipeService.DeleteRecipe(id, userId.Value);

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet]
    public ActionResult<ListOfRecipes> GetRecipes()
        => Ok(_recipeService.GetRecipes());

    [AllowAnonymous]
    [HttpGet("{id}")]
    public ActionResult<RecipeVm> GetRecipe(int id)
        => Ok(_recipeService.GetRecipe(id));
}
