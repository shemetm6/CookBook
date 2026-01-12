using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Models;

namespace CookBook.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IIngredientInRecipeRepository _ingredientInRecipeRepository;
    private readonly IIngredientRepository _ingredientRepository;

    public RecipeService(
        IRecipeRepository recipeRepository,
        IIngredientInRecipeRepository ingredientInRecipeRepository,
        IIngredientRepository ingredientRepository
    )
    {
        _recipeRepository = recipeRepository;
        _ingredientInRecipeRepository = ingredientInRecipeRepository;
        _ingredientRepository = ingredientRepository;
    }

    public int AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string descritption,
        List<IngredientInRecipe> ingredients
    )
    {
        // Добавлено, чтобы выбросить исключение при вводе некорретного Id ингредиента
        // В противном случае рецепт создавался без ингредиентов, если был введен id несуществующего ингредиента
        // (а точнее с теми ингредиентами, которые были перечислены по порядку ДО несуществующего)
        EnsureIngredientsExist(ingredients);

        var recipeId = _recipeRepository.AddRecipe(title, cookTime, timeUnit, descritption);
        foreach (var ingredient in ingredients)
        {
            ingredient.RecipeId = recipeId;
            _ingredientInRecipeRepository.AttachIngredientToRecipe(ingredient);
        }

        return recipeId;
    }

    public void UpdateRecipe(
        int recipeId,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string descritption,
        List<IngredientInRecipe> ingredients
    )
    {
        // Аналогично методу AddRecipe
        EnsureIngredientsExist(ingredients);

        _recipeRepository.UpdateRecipe(recipeId, title, cookTime, timeUnit, descritption);

        var recipe = _recipeRepository.GetRecipe(recipeId);
        foreach (var ingredient in recipe.Ingredients)
        {
            _ingredientInRecipeRepository.RemoveIngredientFromRecipe(ingredient);
        }

        foreach (var ingredient in ingredients)
        {
            _ingredientInRecipeRepository.AttachIngredientToRecipe(ingredient);
        }
    }

    public void DeleteRecipe(int id)
    {
        var recipe = _recipeRepository.GetRecipe(id);
        foreach (var ingredient in recipe.Ingredients)
        {
            _ingredientInRecipeRepository.RemoveIngredientFromRecipe(ingredient);
        }

        _recipeRepository.DeleteRecipe(id);
    }

    public Recipe GetRecipe(int id)
    {
        return _recipeRepository.GetRecipe(id);
    }

    public IReadOnlyList<Recipe> GetRecipes()
    {
        return _recipeRepository.GetRecipes();
    }

    public void RateRecipe(int id, Raiting raiting)
    {
        _recipeRepository.RateRecipe(id, raiting);
    }

    private void EnsureIngredientsExist(List<IngredientInRecipe> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            _ingredientRepository.GetIngredient(ingredient.IngredientId);
        }
    }
}
