using System.ComponentModel.DataAnnotations;

namespace CookBook.Database.Configurations.Database;

// Интерфейс не нужен потому что не реализуется никакая внутренняя логика?
// Т.к. по сути это модель (очень грубо говоря)
public class ApplicationDbContextSettings
{
    // Зачем нужен и атрибут required и модификатор? Модификатор для компилятора, а атрибут для кого?
    [Required]
    // Не понял что за Init. Вроде он говорит, что свойство можно задать только при создании объекта
    // Но по моему это та же логика, которую реализует модификатор required
    public required string Host { get; init; }
    [Required]
    // Здесь не нужен модификатор потому что int это значимый тип и по-умолчанию значение будет 0?
    public int Port { get; init; }
    [Required]
    public required string Username { get; init; }
    [Required]
    public required string Password { get; init; }
    [Required]
    public required string Database { get; init; }

    public string ConnectionString
        => $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database}";
}
