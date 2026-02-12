using System.ComponentModel.DataAnnotations;

namespace CookBook.Configurations;

public record JwtOptions
{
    [Required]
    public required string Issuer { get; init; }
    [Required]
    public required string Audience { get; init; }
    [Required]
    public required string Secret { get; init; }
}
