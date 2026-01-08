using CookBook.Enums;

namespace CookBook.Models;

public class Recipe
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required TimeSpan CookTime { get; set; }
    //public required List<Ingredient> Ingredients { get; set; }
    public required List<string> Ingredients { get; set; }
    public required string Descritption { get; set; }
    public Raiting? Raiting { get; set; }
}
