namespace CookBook.Exceptions;

public class IngredientNotFoundException : Exception
{
    public IngredientNotFoundException(int id) : base($"Ingredient with id = {id} not found!") { }
}
