using CookBook.Abstractions;
using CookBook.Models;
using Microsoft.AspNetCore.Mvc;
using static CookBook.Contracts.Recipe;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    public RecipeController(IRecipeService recipeRepository)
        => _recipeService = recipeRepository;

    [HttpPost]
    public ActionResult<int> AddRecipe(CreateRecipeDto dto)
    {
        var id = _recipeService.AddRecipe(
            dto.Title.Trim(),
            dto.CookTime,
            dto.TimeUnit,
            dto.Description.Trim(),
            dto.Ingredients
                .Select(i => new IngredientInRecipe
                {
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity,
                    Units = i.Units
                })
                .ToList());

        return Ok(id);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateRecipe(int id, UpdateRecipeDto dto)
    {
        _recipeService.UpdateRecipe(
            id,
            dto.Title.Trim(),
            dto.CookTime,
            dto.TimeUnit,
            dto.Description.Trim(),
            dto.Ingredients
                .Select(i => new IngredientInRecipe
                {
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity,
                    Units = i.Units
                })
                .ToList());

        return NoContent();
    }

    [HttpPut("{id}/raiting")]
    public ActionResult RateRecipe(int id, RateRecipeDto dto)
    {
        _recipeService.RateRecipe(id, dto.Raiting);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(int id)
    {
        _recipeService.DeleteRecipe(id);

        return NoContent();
    }

    [HttpGet]
    public ActionResult<ListOfRecipes> GetRecipes()
    {
        var recipes = _recipeService.GetRecipes();


        var listOfRecipes = new ListOfRecipes(recipes
            .Select(recipe =>
            {
                double? averageRaiting = null;

                if (recipe.Raitings.Count > 0)
                    averageRaiting = recipe.Raitings.Average();

                return new RecipesListVm(recipe.Id, recipe.Title, averageRaiting);
            })
            .ToList());

        return Ok(listOfRecipes);
    }
        

    [HttpGet("{id}")]
    public ActionResult<RecipeVm> GetRecipe(int id)
    {
        var recipe = _recipeService.GetRecipe(id);

        double? recipeAverageRaiting = null;

        if (recipe.Raitings.Count > 0)
            recipeAverageRaiting = recipe.Raitings.Average();

        var recipeVm = new RecipeVm(
            recipe.Id,
            recipe.Title,
            recipe.CookTime,
            recipe.Ingredients
                .Select(i => new IngredientsInRecipeVm(i.IngredientId, i.Ingredient!.Name, i.Quantity, i.Units)).ToList(),
            recipe.Descritption,
            recipeAverageRaiting);


        return Ok(recipeVm);
    }
}
