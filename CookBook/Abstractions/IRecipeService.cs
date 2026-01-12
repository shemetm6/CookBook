using CookBook.Enums;
using CookBook.Models;

namespace CookBook.Abstractions
{
    public interface IRecipeService
    {
        public int AddRecipe(
            string title,
            double cookTime,
            TimeUnit timeUnit,
            string descritption,
            List<IngredientInRecipe> ingredients
        );

        public void UpdateRecipe(
            int recipeId,
            string title,
            double cookTime,
            TimeUnit timeUnit,
            string descritption,
            List<IngredientInRecipe> ingredients
        );

        public void RateRecipe(int id, Raiting raiting);

        public void DeleteRecipe(int id);

        public IReadOnlyList<Recipe> GetRecipes();

        public Recipe GetRecipe(int id);
    }
}
