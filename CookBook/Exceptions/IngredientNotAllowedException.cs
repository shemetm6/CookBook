namespace CookBook.Exceptions;

public class IngredientNotAllowedException : Exception
{
    public IngredientNotAllowedException(List<string> invalidIngredients) : base($"Ingredients {string.Join((", ") ,invalidIngredients)} not allowed!") { }
}
