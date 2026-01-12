namespace CookBook.Models;

public class Ingredient
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<IngredientInRecipe> Recipes { get; set; } = [];
}
