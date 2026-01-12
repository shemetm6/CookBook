using CookBook.Models;

namespace CookBook.Abstractions;

public interface IIngredientInRecipeRepository
{
    public void AttachIngredientToRecipe(IngredientInRecipe ingredientInRecipe);
    public void RemoveIngredientFromRecipe(IngredientInRecipe ingredientInRecipe);
}
