using CookBook.Models;

namespace CookBook.Abstractions;

public interface IIngredientRepository
{
    public int AddIngredient(string name);
    public void AddRecipeToIngredient(IngredientInRecipe ingredientInRecipe);
    public IReadOnlyList<Ingredient> GetIngredients();
}
