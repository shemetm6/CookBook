using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Models;
using CookBook.Services;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController : ControllerBase
{
    private readonly IIngredientRepository _ingredientRepository;

    public IngredientController(IIngredientRepository ingredientRepository)
        => _ingredientRepository = ingredientRepository;

    [HttpPost]
    public ActionResult<int> AddRecipe(string name)
    {
        var ingredientId = _ingredientRepository.AddIngredient(name.Trim());

        return Ok(ingredientId);
    }

    [HttpGet]
    public ActionResult<List<Ingredient>> GetIngredients()
        => Ok(_ingredientRepository.GetIngredients());
}
