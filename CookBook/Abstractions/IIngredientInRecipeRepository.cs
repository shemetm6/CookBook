using CookBook.Enums;
using CookBook.Models;

namespace CookBook.Abstractions;

public interface IIngredientInRecipeRepository
{
    public List<IngredientInRecipe> HandleIngredientInRecipeList(
        int recipeId,
        List<IngredientInRecipe> ingredientsInRecipeList);
}
