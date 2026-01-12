using CookBook.Enums;
using CookBook.Models;

namespace CookBook.Exceptions;

public class IngredientNotAllowedException : Exception
{
    public IngredientNotAllowedException(int ingredientId, List<Ingredient> allowedIngredients)
        : base(BuildMessage(ingredientId, allowedIngredients)) { }

    private static string BuildMessage(int invalidIngredientId, List<Ingredient> allowedIngredients)
    {
        var allowedIngredientsIds = string.Join(", ", allowedIngredients.Select(i => i.Id).ToList());

        if (allowedIngredients.Count == 0)
            return $"No ingredients available!\n" +
                   $"Create ingredients before creating recipe.";

        return $"Ingredient with id = {invalidIngredientId} are not allowed!\n" +
               $"List of available ingredient IDs: {allowedIngredientsIds}.";
    }
}
