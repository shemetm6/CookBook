using CookBook.Contracts;
using CookBook.Enums;

namespace CookBook.Abstractions;

public interface IRecipeService
{
    Task<int> AddRecipeAsync(CreateRecipeDto dto, int userId);
    Task UpdateRecipeAsync(int id, UpdateRecipeDto dto, int userId);
    Task RateRecipeAsync(int id, RateRecipeDto dto, int userId);
    Task DeleteRecipeAsync(int id, int userId);
    Task<ListOfRecipes> GetRecipesAsync(
        string? title,
        double? minRating,
        string? author,
        RecipeSortBy? sortBy,
        bool? descending
        );
    Task<RecipeVm> GetRecipeAsync(int id);
}
