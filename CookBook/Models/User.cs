namespace CookBook.Models;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public byte[] Password { get; set; } = null!;
    public ICollection<Recipe> Recipes { get; set; } = [];
    public ICollection<Rating> Ratings { get; set; } = [];
}
