using System.ComponentModel.DataAnnotations;

namespace CookBook.Configurations;

// Не стал использовать сокращенный (positional) синтаксис по причине описанной в ApplicationDbContextSettings
public record JwtOptions
{
    [Required]
    public required string Issuer { get; init; }
    [Required]
    public required string Audience { get; init; }
    [Required]
    public required string Secret { get; init; }
}
