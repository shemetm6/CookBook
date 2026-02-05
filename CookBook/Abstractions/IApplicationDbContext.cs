using CookBook.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Abstractions;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Ingredient> Ingredients { get; }
    DbSet<Recipe> Recipes { get; }
    DbSet<Rating> Ratings { get; set; }
    DbSet<IngredientInRecipe> IngredientsInRecipes { get; }
    DbSet<JwtToken> JwtTokens { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }

    int SaveChanges();
}
