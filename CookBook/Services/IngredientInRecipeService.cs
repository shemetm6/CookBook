using CookBook.Abstractions;
using CookBook.Models;
using static CookBook.Contracts.Recipe;

namespace CookBook.Services;

public class IngredientInRecipeService : IIngredientInRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IIngredientInRecipeRepository _ingredientInRecipeRepository;


    public IngredientInRecipeService(
        IRecipeRepository recipeRepository,
        IIngredientInRecipeRepository ingredientInRecipeRepository,
        IIngredientRepository ingredientRepository
    )
    {
        _recipeRepository = recipeRepository;
        _ingredientInRecipeRepository = ingredientInRecipeRepository;
    }

    public void AttachAllIngredientsToRecipe(int recipeId, IEnumerable<IngredientInRecipeCreateVm> dto)
    {
        foreach (var ingredientInRecipeVm in dto)
        {
            var ingredientInRecipe = new IngredientInRecipe
            {
                RecipeId = recipeId,
                IngredientId = ingredientInRecipeVm.IngredientId,
                Quantity = ingredientInRecipeVm.Quantity,
                Units = ingredientInRecipeVm.Units
            };
            _ingredientInRecipeRepository.AttachIngredientToRecipe(ingredientInRecipe);
        }
    }

    public void ReplaceAllIngredientsInRecipe(int recipeId, IEnumerable<IngredientInRecipeCreateVm> dto)
    {
        RemoveAllIngredientFromRecipe(recipeId);

        AttachAllIngredientsToRecipe(recipeId, dto);
    }

    private void RemoveAllIngredientFromRecipe(int recipeId)
    {
        var recipe = _recipeRepository.GetRecipe(recipeId);

        foreach (var ingredient in recipe.Ingredients.ToList())
        {
            _ingredientInRecipeRepository.RemoveIngredientFromRecipe(ingredient);
        }
    }
}
