using CookBook.Models;

namespace CookBook.Abstractions;

public interface IIngredientRepository
{
    public int AddIngredient(string name);
    public Ingredient GetIngredient(int id);
    public IReadOnlyList<Ingredient> GetIngredients();
}
