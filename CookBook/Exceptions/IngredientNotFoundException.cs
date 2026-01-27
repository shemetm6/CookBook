namespace CookBook.Exceptions;

public class IngredientNotFoundException : Exception
{
    public IngredientNotFoundException(int id)
        : base($"Ingredient with id = {id} not found!") { }
    public IngredientNotFoundException(IEnumerable<int> ids)
        : base($"Ingredient with id(s) = {string.Join(", ", ids)} not found!") { }
}
