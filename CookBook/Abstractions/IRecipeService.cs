using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IRecipeService
{
    int AddRecipe(CreateRecipeDto dto, int userId);
    void UpdateRecipe(int id, UpdateRecipeDto dto, int userId);
    void RateRecipe(int id, RateRecipeDto dto);
    void DeleteRecipe(int id, int userId);
    ListOfRecipes GetRecipes();
    RecipeVm GetRecipe(int id);
}
