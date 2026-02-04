using CookBook.Abstractions;
using CookBook.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

public class IngredientsController : BaseController
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
