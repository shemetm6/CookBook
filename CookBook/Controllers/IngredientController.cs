using CookBook.Abstractions;
using Microsoft.AspNetCore.Mvc;
using static CookBook.Contracts.Ingredient;


namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController : ControllerBase
{
    private readonly IIngredientRepository _ingredientRepository;

    public IngredientController(IIngredientRepository ingredientRepository)
        => _ingredientRepository = ingredientRepository;

    [HttpPost]
    public ActionResult<int> CreateIngredient(CreateIngredientDto dto)
    {
        var ingredientId = _ingredientRepository.AddIngredient(dto);

        return Ok(ingredientId);
    }

    [HttpGet]
    public ActionResult<ListOfIngredients> GetIngredients()
    {
        var ingredients = _ingredientRepository.GetIngredients();

        return Ok(ingredients);
    }
}
