namespace CookBook.Exceptions;

public class RecipeIdDuplicateException : Exception
{
    public RecipeIdDuplicateException(int id) : base($"Recipe with id = {id} already exists!") { }
}
