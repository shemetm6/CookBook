using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IRecipeService
{
    int AddRecipe(CreateRecipeDto dto);
    void UpdateRecipe(int id, UpdateRecipeDto dto);
    void RateRecipe(int id, RateRecipeDto dto);
    void DeleteRecipe(int id);
    ListOfRecipes GetRecipes();
    RecipeVm GetRecipe(int id);
}
