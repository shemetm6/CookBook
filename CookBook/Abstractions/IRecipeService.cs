using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IRecipeService
{
    int AddRecipe(CreateRecipeDto dto, int userId);
    void UpdateRecipe(int id, UpdateRecipeDto dto, int userId);
    void RateRecipe(int id, RateRecipeDto dto, int userId);
    void DeleteRecipe(int id, int userId);
    ListOfRecipes GetRecipes(
        string? title,
        double? minRating,
        string? author,
        string? sortBy,
        bool? descending
        );
    RecipeVm GetRecipe(int id);
}
