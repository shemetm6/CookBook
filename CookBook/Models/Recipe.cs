namespace CookBook.Models;

public class Recipe
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required TimeSpan CookTime { get; set; }
    public ICollection<IngredientInRecipe> Ingredients { get; set; } = [];
    public required string Description { get; set; }
    public ICollection<int> Ratings { get; set; } = [];
}
