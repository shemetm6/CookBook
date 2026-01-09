using CookBook.Enums;
using CookBook.Models;

namespace CookBook.Abstractions;

public interface IIngredientInRecipeRepository
{
    public List<IngredientInRecipe> AddIngredientsToRecipe(
    int recipeId,
    List<int> ingredientIds,
    double quantity,
    QuantityUnit units);
}
