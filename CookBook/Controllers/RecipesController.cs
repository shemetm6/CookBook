using CookBook.Abstractions;
using CookBook.Contracts;
using CookBook.Extensions;
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
        // Нейронка предложила вынести поиск userId в метод BaseController
        // Например GetCurrentUserId. Нормальная идея?
        // Просто я хз, как BaseController обычно выглядит
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

    /* (todo) Внедрить рейтинг
    [HttpPut("{id}/raiting")]
    public ActionResult RateRecipe(int id, RateRecipeDto dto)
    {
        _recipeService.RateRecipe(id, dto);

        return NoContent();
    }
    */

    // Произошла какая то хуйня с policy.
    // Т.е. если мне использовать ее, то тогда нужно указывать свой id в методах Delete/Update.
    // При том, что если указать чужой id, то удалить/обновить рецепт мне не дадут.
    // И у нас есть HttpContext из которого можно извлечь Id без участия юзера, главное чтобы он был залогинен
    // Потому я не понял зачем использовать атрибут с политикой и убрал его, чтобы не заполнять лишнее поле в сваггере
    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(int id)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        _recipeService.DeleteRecipe(id, userId.Value);

        return NoContent();
    }

    [HttpGet]
    public ActionResult<ListOfRecipes> GetRecipes()
        => Ok(_recipeService.GetRecipes());

    [HttpGet("{id}")]
    public ActionResult<RecipeVm> GetRecipe(int id)
        => Ok(_recipeService.GetRecipe(id));
}
