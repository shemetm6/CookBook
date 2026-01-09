using CookBook.Enums;

namespace CookBook.Exceptions;

public class IngredientNotAllowedException : Exception
{
    public IngredientNotAllowedException(List<int> invalidIngredients, List<int> allowedIngredients)
        : base(BuildMessage(invalidIngredients, allowedIngredients)) { }

    private static string BuildMessage(List<int> invalidIngredients, List<int> allowedIngredients)
    {
        var invalid = string.Join(", ", invalidIngredients);
        var allowed = string.Join(", ", allowedIngredients);

        return $"Ingredients with following IDs are not allowed: {invalid}.\n" +
               $"List of available ingredient IDs: {allowed}.";
    }
}
