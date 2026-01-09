using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Models;

namespace CookBook.Services;

public class IngredientRepository : IIngredientRepository
{
    private readonly List<Ingredient> _ingredients = new();

    public int AddIngredient(string name)
    {
        var ingredientId = _ingredients.Count;

        _ingredients.Add(new Ingredient()
        {
            Id = ingredientId,
            Name = name,
        });

        return ingredientId;
    }

    public void AddRecipeToIngredient(IngredientInRecipe ingredientInRecipe)
    {
        var ingredient = _ingredients.First(i => i.Id == ingredientInRecipe.IgredientId);
        ingredient.Recipes.Add(ingredientInRecipe);
    }

    public IReadOnlyList<Ingredient> GetIngredients() => _ingredients;

}
