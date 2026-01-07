using CookBook.Enums;

namespace CookBook.Models;

public class Recipe
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required int CookTimeInMinutes { get; set; } // Хотел использовать TimeSpan, но возникли проблемы с использованием в patch запросе

    //public List<Ingredient> Ingredients { get; set; } // Я не вывез каждый раз заполнять огромную простыню в body + По условию задачи у нас список ингридиентов ограничен, так что для упрощения заменяю на string

    public required List<string> Ingredients { get; set; }
    public required string Descritption { get; set; }
    public Raiting Raiting { get; set; }
}
