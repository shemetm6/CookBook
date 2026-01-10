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

    // Кароуч. Я пробовал передавать ingredientIds строкой.
    // Из плюсов: объект IngredientInRecipe создавался в соответствующем репозитории. Из минусов ед. измерения были одни на весь список ингредиентов.
    // Чатгпт посоветовал перекатиться на dto. Попробовал. Увидел, что ВЕСЬ запрос теперь передается в request body. Не оценил
    // Понял, что можно передать список ингредиентов в теле запроса.
    // Из минусов: их дохуя. Плохо, что пользователь сам ручками создает объект, наверное это должно происходить в соответствующем репозитории
    // Плохо, что я не могу выкинуть кастомное исключение при ошибке в timeUnit. Даже, если выбросить исключение в котроллере, 400ка меня обгоняет
    // Плохо, что я породил такое чудовище как HandleIngredientInRecipeList, его реально можно переименовывать в DoCoolThings, делает всё и сразу.
    // Из плюсов: мне нравится форма для создания рецепта в сваггере. Всё что возможно пишется в полях, а список ингредиентов в теле запроса
    public int AddRecipe(
        string title,
        double cookTime,
        TimeUnit timeUnit,
        List<IngredientInRecipe> ingredients,
        string descritption)
    {
        var recipeId = _recipes.Count;
        var ingredientInRecipeList= _ingredientInRecipeRepository.HandleIngredientInRecipeList(recipeId, ingredients);

        _recipes.Add(new Recipe
        {
            Id = recipeId,
            Title = title,
            CookTime = _timeConverter.Convert(cookTime, timeUnit),
            Ingredients = ingredients,
            Descritption = descritption
        });

        return recipeId;
    }

    public void UpdateRecipe(
        int recipeId,
        string title,
        double cookTime,
        TimeUnit timeUnit,
        List<IngredientInRecipe> ingredients,
        string descritption)
    {
        var recipe = TryGetRecipeAndThrowIfNotFound(recipeId);
        var ingredientInRecipeList = _ingredientInRecipeRepository.HandleIngredientInRecipeList(recipeId, ingredients);

        recipe.Title = title;
        recipe.CookTime = _timeConverter.Convert(cookTime, timeUnit);
        recipe.Descritption = descritption;
        recipe.Ingredients = _ingredientInRecipeRepository.HandleIngredientInRecipeList(recipeId, ingredients);
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
}
