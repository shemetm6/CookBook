using CookBook.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Abstractions;

public interface IApplicationDbContext
{
    DbSet<User> Users { get;}
    DbSet<Ingredient> Ingredients { get; }
    DbSet<Recipe> Recipes { get; }
    DbSet<Rating> Ratings { get;}
    DbSet<IngredientInRecipe> IngredientsInRecipes { get; }
    DbSet<JwtToken> JwtTokens { get;}
    DbSet<RefreshToken> RefreshTokens { get;}

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
