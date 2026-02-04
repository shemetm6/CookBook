using CookBook.Models;
using CookBook.Abstractions;
using CookBook.Configurations.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CookBook.Database;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ApplicationDbContextSettings _dbContextSettings;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ApplicationDbContext> _logger;

    public DbSet<User> Users { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<IngredientInRecipe> IngredientsInRecipes { get; set; }
    public virtual DbSet<JwtToken> JwtTokens { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<ApplicationDbContextSettings> dbContextSettings,
        IWebHostEnvironment environment,
        ILogger<ApplicationDbContext> logger
        ) : base(options)
    {
        _dbContextSettings = dbContextSettings.Value;
        _environment = environment;
        _logger = logger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(_dbContextSettings.ConnectionString);
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Debug);

        if (_environment.IsDevelopment())
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(message
                => _logger.LogInformation(message), LogLevel.Information);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}
