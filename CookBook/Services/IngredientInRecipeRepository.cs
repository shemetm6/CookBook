using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Models;
using System.Net.NetworkInformation;

namespace CookBook.Services;

public class IngredientInRecipeRepository : IIngredientInRecipeRepository
{
    private readonly List<IngredientInRecipe> _ingredientInRecipe = new();
    private readonly IIngredientRepository _ingredientRepository;

    public IngredientInRecipeRepository(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    // Ужасный метод, пока думаю как подправить. Можно разбить на Validate и приватный SaveToRepositories,
    // но он всё равно будет присваивать recipeId, что не вписывается ни в один из вышеперечисленных методов
    public List<IngredientInRecipe> HandleIngredientInRecipeList(
        int recipeId,
        List<IngredientInRecipe> ingredientsInRecipeList)
    {
        ThrowIfIngredientsNotAllowed(ingredientsInRecipeList);

        foreach (var ingredientInRecipe in ingredientsInRecipeList)
        {
            ingredientInRecipe.SetRecipeId(recipeId);
            _ingredientInRecipe.Add(ingredientInRecipe);
            _ingredientRepository.AddRecipeToIngredient(ingredientInRecipe);
        }

        return ingredientsInRecipeList;
    }

    private void ThrowIfIngredientsNotAllowed(List<IngredientInRecipe> ingredientsInRecipeList)
    {
        var allowedIngredientIds = _ingredientRepository
            .GetIngredients()
            .Select(i => i.Id)
            .ToList();

        var invalidIngredientIds = ingredientsInRecipeList
            .Select(i => i.IgredientId)
            .Where(i => !allowedIngredientIds.Contains(i))
            .ToList();

        if (invalidIngredientIds.Count != 0)
            throw new IngredientNotAllowedException(invalidIngredientIds, allowedIngredientIds);
    }
}

