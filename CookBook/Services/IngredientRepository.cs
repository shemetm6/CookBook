using CookBook.Abstractions;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Models;

namespace CookBook.Services;

public class IngredientRepository : IIngredientRepository
{
    private readonly List<Ingredient> _ingredients = [];

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

    public Ingredient GetIngredient(int id) 
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == id);

        return ingredient ?? throw new IngredientNotAllowedException(id, _ingredients);
    }

    public IReadOnlyList<Ingredient> GetIngredients() => _ingredients;
}
