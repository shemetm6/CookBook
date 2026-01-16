using CookBook.Models;

namespace CookBook.Abstractions;

public interface IIngredientInRecipeRepository
{
    void AttachIngredientToRecipe(IngredientInRecipe ingredientInRecipe);
    void RemoveIngredientFromRecipe(IngredientInRecipe ingredientInRecipe);
}
