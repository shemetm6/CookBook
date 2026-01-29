using CookBook.Abstractions;
using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CookBook.Configurations.Database;

namespace CookBook.Database;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public readonly ApplicationDbContextSettings _dbContextSettings;

    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<IngredientInRecipe> IngredientsInRecipes { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<ApplicationDbContextSettings> dbContextSettings
        ) : base(options)
    {
        _dbContextSettings = dbContextSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(_dbContextSettings.ConnectionString);
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Debug);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
