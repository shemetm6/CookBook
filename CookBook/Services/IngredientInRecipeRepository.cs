using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Models;

namespace CookBook.Services;

public class IngredientInRecipeRepository : IIngredientInRecipeRepository
{
    private readonly List<IngredientInRecipe> _ingredientInRecipe = new();
    private readonly IIngredientRepository _ingredientRepository;

    public IngredientInRecipeRepository(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    public List<IngredientInRecipe> AddIngredientsToRecipe(
        int recipeId,
        List<int> ingredientIds,
        double quantity,
        QuantityUnit units)
    {
        ThrowIfIngredientsNotAllowed(ingredientIds);

        var ingredients = new List<IngredientInRecipe>();

        foreach (var id in ingredientIds)
        {
            var ingridientInRecipe = new IngredientInRecipe
            {
                IgredientId = id,
                RecipeId = recipeId,
                Quantity = quantity,
                Units = units
            };
            ingredients.Add(ingridientInRecipe);
            _ingredientInRecipe.Add(ingridientInRecipe);
            _ingredientRepository.AddRecipeToIngredient(ingridientInRecipe); // (!)Не уверен. Писал об этом в Ingredient.cs
        }

        return ingredients;
    }

    private void ThrowIfIngredientsNotAllowed(List<int> ingredientIds)
    {
        var allowedIngredientIds = _ingredientRepository
            .GetIngredients()
            .Select(i => i.Id)
            .ToList();

        var invalidIngredientIds = ingredientIds
            .Where(i => !allowedIngredientIds.Contains(i))
            .ToList();

        if (invalidIngredientIds.Count != 0)
            throw new IngredientNotAllowedException(invalidIngredientIds, allowedIngredientIds);
    }
}

