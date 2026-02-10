using System.ComponentModel.DataAnnotations;

namespace CookBook.Configurations.Database;

// Клод говорит, что атрибут Required не работает с параметрами конструктора, а работает только со свойствами.
// Поэтому сокращенный синтаксис использовать не стал.
public record ApplicationDbContextSettings
{
    [Required]
    public required string Host { get; init; }
    [Required]
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
