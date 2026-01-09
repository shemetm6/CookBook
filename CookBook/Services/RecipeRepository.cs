using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Services;

public class RecipeRepository : IRecipeRepository
{
    private readonly List<Recipe> _recipes = new();
    private readonly ITimeConverter _timeConverter;
    // Вот на этом моменте вероятно что-то пошло не так, потому что ты писал, что IngredientInRecipeRepository должен хранить остальные два репозитория
    // У меня эта иерархия выглядит по другому
    private readonly IIngredientInRecipeRepository _ingredientInRecipeRepository; 

    public RecipeRepository(
        ITimeConverter timeConverter, 
        IIngredientInRecipeRepository ingredientInRecipeRepository)
    {
        _timeConverter = timeConverter;
        _ingredientInRecipeRepository = ingredientInRecipeRepository;
    }

    // (!) У меня одно количество и единицы измерения для всех ингредиентов
    // И 0 идей как это исправить.
    // Как ты писал, при создании рецепта должно произойти следущее: "У пользователя открылась форма поверх рецепта, в которую он ввел инфу об ингредиенте"
    // Я такого в сваггере еще не видел и как реализовывать тоже хз
    // Чатгпт говорит про dto
    public int AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string ingredientIds,
        double quantity,
        QuantityUnit units,
        string descritption)
    {
        var recipeId = _recipes.Count;
        var parsedIngredients = ParseIngredients(ingredientIds);
        var ingredientInRecipeList= _ingredientInRecipeRepository.AddIngredientsToRecipe(recipeId, parsedIngredients, quantity, units);

        _recipes.Add(new Recipe
        {
            Id = recipeId,
            Title = title,
            CookTime = _timeConverter.Convert(cookTime, timeUnit),
            Ingredients = ingredientInRecipeList,
            Descritption = descritption
        });

        return recipeId;
    }

    public void UpdateRecipe(
        int recipeId,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        string ingredients,
        double quantity,
        QuantityUnit units,
        string descritption)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(recipeId);
        var parsedIngredients = ParseIngredients(ingredients);

        recipe.Title = title;
        recipe.CookTime = _timeConverter.Convert(cookTime, timeUnit);
        recipe.Descritption = descritption;
        recipe.Ingredients = _ingredientInRecipeRepository.AddIngredientsToRecipe(recipeId, parsedIngredients, quantity, units);
    }

    public void RateRecipe(int id, Raiting raiting)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);
        recipe.Raitings.Add((int)raiting);
    }

    public void DeleteRecipe(int id)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(id);
        _recipes.Remove(recipe);
    }

    public IReadOnlyList<Recipe> GetRecipes() => _recipes;

    public Recipe GetRecipe(int id) => TryGetRecipeAndThrowIfNotFound(id);

    private Recipe TryGetRecipeAndThrowIfNotFound(int id)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id == id);

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        return recipe;
    }

    private static List<int> ParseIngredients(string ingredientsInput)
    {
        return ingredientsInput
            .Split(',')
            .Select(i => i.Trim())
            .Select(i => int.Parse(i))
            .ToList();
    }
}
