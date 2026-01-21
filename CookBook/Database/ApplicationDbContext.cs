using CookBook.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<IngredientInRecipe> IngredientsInRecipes { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IngredientInRecipe>()
            .HasKey(ir => new { ir.IngredientId, ir.RecipeId });

        base.OnModelCreating(modelBuilder);
    }
}

