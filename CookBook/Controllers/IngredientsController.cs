using CookBook.Abstractions;
using CookBook.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

public class IngredientsController : BaseController
{
    private readonly IIngredientService _ingredientService;

    public IngredientsController(IIngredientService ingredientService)
        => _ingredientService = ingredientService;

    [HttpPost]
    public async Task<ActionResult<int>> CreateIngredient(CreateIngredientDto dto)
    {
        var ingredient = await _ingredientService.AddIngredientAsync(dto);

        return Ok(ingredient);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ListOfIngredients>> GetIngredients()
    {
        var ingredients = await _ingredientService.GetIngredientsAsync();

        return Ok(ingredients);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientVm>> GetIngredient(int id)
    {
        var ingredient = await _ingredientService.GetIngredientAsync(id);

        return Ok(ingredient);
    }
}
