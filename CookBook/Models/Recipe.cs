using CookBook.Enums;

namespace CookBook.Models;

public class Recipe
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required int CookTimeInMinutes { get; set; } // Стоит ли использовать TimeSpan вместо int?

    // У меня была великолепная идея с моделью Ingridient, у которой есть св-ва Name, Quantity, Units и отдельным enum UnitsOfMesurement.
    // Но я не вывез каждый раз ручками заполнять тело запроса в сваггере.
    //public required List<Ingredient> Ingredients { get; set; }

    public required List<string> Ingredients { get; set; }
    public required string Descritption { get; set; }
    public Raiting? Raiting { get; set; }
}
