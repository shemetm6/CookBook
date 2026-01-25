using CookBook.Abstractions;
using Microsoft.AspNetCore.Mvc;
using CookBook.Contracts;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientService _ingredientService;

    public IngredientsController(IIngredientService ingredientService)
        => _ingredientService = ingredientService;

    [HttpPost]
    public ActionResult<int> CreateIngredient(CreateIngredientDto dto) 
        => Ok(_ingredientService.AddIngredient(dto));
    
    [HttpGet]
    public ActionResult<ListOfIngredients> GetIngredients() 
        => Ok(_ingredientService.GetIngredients());

    [HttpGet("{id}")]
    public ActionResult<IngredientVm> GetIngredient(int id)
        => Ok(_ingredientService.GetIngredient(id));
}
