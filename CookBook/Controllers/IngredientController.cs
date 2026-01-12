using CookBook.Abstractions;
using CookBook.Contracts;
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
        var ingredientId = _ingredientRepository.AddIngredient(dto.Name.Trim());

        return Ok(ingredientId);
    }

    [HttpGet]
    public ActionResult<ListOfIngredients> GetIngredients()
    {
        var ingredients = _ingredientRepository.GetIngredients();

        var ingredientVms = ingredients
            .Select(i => new IngredientListVm(
                i.Id,
                i.Name,
                i.Recipes.Select(r => new RecipeInIngredientVm(r.RecipeId, r.Recipe!.Title)).ToList()))
            .ToList();

        return Ok(new ListOfIngredients(ingredientVms));
    }
}
