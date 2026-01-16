using CookBook.Abstractions;
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

    public Ingredient GetIngredient(int id) => TryGetIngredientAndThrowIfNotFound(id);

    public IReadOnlyList<Ingredient> GetIngredients() => _ingredients;

    private Ingredient TryGetIngredientAndThrowIfNotFound(int id)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == id);

        return ingredient ?? throw new IngredientNotFoundException(id);
    }
}
