namespace CookBook.Exceptions;

public class RecipeNotFoundException : Exception
{
    public RecipeNotFoundException(int id) : base($"Recipe with id = {id} not found!") { }
}
