using CookBook.Enums;

namespace CookBook.Exceptions;

public class IngredientNotAllowedException : Exception
{
    public IngredientNotAllowedException(List<string> invalidIngredients) : base(BuildMessage(invalidIngredients))
    { }

    private static string BuildMessage(List<string> invalidIngredients)
    {
        var allowedIngredients = Enum.GetNames<AllowedIngredients>().ToList();

        var invalid = string.Join(", ", invalidIngredients);
        var allowed = string.Join(", ", allowedIngredients);

        return $"Following ingredients are not allowed: {invalid}.\n" +
               $"List of allowed ingredients: {allowed}.";
    }
}
