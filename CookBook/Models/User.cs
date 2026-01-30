namespace CookBook.Models;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public ICollection<Recipe> Recipes { get; set; } = [];
}
