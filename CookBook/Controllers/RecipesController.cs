using CookBook.Abstractions;
using CookBook.Contracts;
using CookBook.Enums;
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
    public async Task<ActionResult<int>> AddRecipe(CreateRecipeDto dto)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();
        
        if (userId is null)
            return Unauthorized();

        var recipeId = await _recipeService.AddRecipeAsync(dto, userId.Value);

        return Ok(recipeId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRecipe(int id, UpdateRecipeDto dto)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        await _recipeService.UpdateRecipeAsync(id, dto, userId.Value);

        return NoContent();
    }

    [HttpPut("{id}/raiting")]
    public async Task<ActionResult> RateRecipe(int id, RateRecipeDto dto)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        await _recipeService.RateRecipeAsync(id, dto, userId.Value);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRecipe(int id)
    {
        var userId = HttpContext.ExtractUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        await _recipeService.DeleteRecipeAsync(id, userId.Value);

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ListOfRecipes>> GetRecipes(
        string? title,
        double? minRating,
        string? author,
        RecipeSortBy? sortBy,
        bool? descending
        )
    {
        var recipes = await _recipeService.GetRecipesAsync(title, minRating, author, sortBy, descending);

        return Ok(recipes);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeVm>> GetRecipe(int id)
    {
        var recipe = await _recipeService.GetRecipeAsync(id);

        return Ok(recipe);
    }
}
