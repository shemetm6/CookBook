using CookBook.Abstractions;
using CookBook.Models;

namespace CookBook.Services;

public class IngredientInRecipeRepository : IIngredientInRecipeRepository
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IRecipeRepository _recipeRepository;

    public IngredientInRecipeRepository(
        IIngredientRepository ingredientRepository,
        IRecipeRepository recipeRepository
    )
    {
        _ingredientRepository = ingredientRepository;
        _recipeRepository = recipeRepository;
    }

    public void AttachIngredientToRecipe(IngredientInRecipe ingredientInRecipe)
    {
        var ingredient = _ingredientRepository.GetIngredient(ingredientInRecipe.IngredientId);
        var recipe = _recipeRepository.GetRecipe(ingredientInRecipe.RecipeId);

        ingredient.Recipes.Add(ingredientInRecipe);
        recipe.Ingredients.Add(ingredientInRecipe);
    }

    public void RemoveIngredientFromRecipe(IngredientInRecipe ingredientInRecipe)
    {
        var ingredient = _ingredientRepository.GetIngredient(ingredientInRecipe.IngredientId);
        var recipe = _recipeRepository.GetRecipe(ingredientInRecipe.RecipeId);

        var toRemoveInIngredient = ingredient.Recipes.First(
            i => i.IngredientId == ingredientInRecipe.IngredientId && i.RecipeId == ingredientInRecipe.RecipeId);
        var toRemoveInRecipe = recipe.Ingredients.First(
            r => r.IngredientId == ingredientInRecipe.IngredientId && r.RecipeId == ingredientInRecipe.RecipeId);

        ingredient.Recipes.Remove(toRemoveInIngredient);
        recipe.Ingredients.Remove(toRemoveInRecipe);
    }
}

