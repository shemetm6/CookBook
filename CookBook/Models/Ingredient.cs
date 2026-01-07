using CookBook.Enums;

namespace CookBook.Models;
// Временно не используется
public class Ingredient
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public UnitOfMeasurement UnitOfMeasurement { get; set; }
}
