using CookBook.Models;

namespace CookBook.Abstractions;

public interface IIngredientRepository
{
    int AddIngredient(string name);
    Ingredient GetIngredient(int id);
    IReadOnlyList<Ingredient> GetIngredients();
}
