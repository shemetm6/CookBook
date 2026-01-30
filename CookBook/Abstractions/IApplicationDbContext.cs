using CookBook.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Abstractions;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Ingredient> Ingredients { get; }
    public DbSet<Recipe> Recipes { get; }
    public DbSet<IngredientInRecipe> IngredientsInRecipes { get; }

    public int SaveChanges();
}
