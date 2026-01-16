using static CookBook.Contracts.Recipe;

namespace CookBook.Abstractions;

public interface IIngredientInRecipeService
{
    void AttachAllIngredientsToRecipe(int recipeId, IEnumerable<IngredientInRecipeCreateVm> dto);
    void ReplaceAllIngredientsInRecipe(int recipeId, IEnumerable<IngredientInRecipeCreateVm> dto);
}
