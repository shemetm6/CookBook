using CookBook.Enums;

namespace CookBook.Models;

public class IngredientInRecipe
{
    public int IngredientId { get; set; }
    public int RecipeId { get; set; }
    public double Quantity { get; set; }
    public QuantityUnit Units { get; set; }
    public Recipe? Recipe { get; set; }
    public Ingredient? Ingredient { get; set; }
}
