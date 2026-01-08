using CookBook.Enums;
using CookBook.Models;
using CookBook.Exceptions;
using CookBook.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    public RecipeController(IRecipeRepository recipeRepository)
        => _recipeRepository = recipeRepository;

    [HttpPost]
    public ActionResult<int> AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string ingredients,
        string descritption)
    {
        var id = _recipeRepository.AddRecipe(title.Trim(), cookTime, timeUnit, ingredients, descritption.Trim());

        return Ok(id);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateRecipe(
        int id,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string ingredients,
        string descritption)
    {
        _recipeRepository.UpdateRecipe(id, title.Trim(), cookTime, timeUnit, ingredients, descritption.Trim());

        return NoContent();
    }

    [HttpPut("{id}/rating")]
    public ActionResult RateRecipe(int id, Raiting raiting)
    {
        _recipeRepository.RateRecipe(id, raiting);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(int id)
    {
        _recipeRepository.DeleteRecipe(id);

        return NoContent();
    }

    [HttpGet]
    public ActionResult<List<Recipe>> GetRecipes() 
        => Ok(_recipeRepository.GetRecipes());

    [HttpGet("{id}")]
    public ActionResult<Recipe> GetRecipe(int id) 
        => Ok(_recipeRepository.GetRecipe(id));
}
