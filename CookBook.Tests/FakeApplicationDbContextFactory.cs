using CookBook.Database;
using CookBook.Models;
using CookBook.Utils;
using CookBook.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Tests;

public static class FakeApplicationDbContextFactory
{
    public const int UserAId = 1;
    public const int UserBId = 2;

    public const int IngredientAId = 1;
    public const int IngredientBId = 2;

    public const int RecipeIdToRead = 1;
    public const int RecipeIdToUpdate = 2;
    public const int RecipeIdToDelete = 3;
    public const int RecipeIdToRate = 4;

    public static (ApplicationDbContext context, SqliteConnection connection) Create()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.AddRange(
            new User
            {
                Id = UserAId,
                Login = "A",
                Password = PasswordHasher.HashPassword("Pass")
            },
            new User
            {
                Id = UserBId,
                Login = "B",
                Password = PasswordHasher.HashPassword("Pass")
            });

        context.Ingredients.AddRange(
            new Ingredient 
            {
                Id = IngredientAId,
                Name = "IngredientA"
            },
            new Ingredient
            {
                Id = IngredientBId,
                Name = "IngredientB"
            }
            );

        context.Recipes.AddRange(
            new Recipe
            {
                Id = RecipeIdToRead,
                Title = "RecipeToRead",
                CookTime = TimeSpan.FromSeconds(30),
                Description = "Desc1",
                UserId = UserAId,
                Ingredients = new List<IngredientInRecipe>
                {
                    new IngredientInRecipe
                    {
                        IngredientId = IngredientAId,
                        Quantity = 2,
                        Units = QuantityUnit.Cups
                    }
                }
            },
            new Recipe
            {
                Id = RecipeIdToDelete,
                Title = "RecipeToDelete",
                CookTime = TimeSpan.FromMinutes(3),
                Description = "Desc2",
                UserId = UserAId,
                Ingredients = new List<IngredientInRecipe>
                {
                    new IngredientInRecipe
                    {
                        IngredientId = IngredientBId,
                        Quantity = 4,
                        Units = QuantityUnit.Teaspoons
                    }
                }
            },
            new Recipe
            {
                Id = RecipeIdToUpdate,
                Title = "RecipeToUpdate",
                CookTime = TimeSpan.FromHours(0.3),
                Description = "Desc3",
                UserId = UserBId,
                Ingredients = new List<IngredientInRecipe>
                {
                    new IngredientInRecipe
                    {
                        IngredientId = IngredientAId,
                        Quantity = 15,
                        Units = QuantityUnit.Milliliters
                    },
                    new IngredientInRecipe
                    {
                        IngredientId = IngredientBId,
                        Quantity = 4,
                        Units = QuantityUnit.Units
                    }
                }
            },
            new Recipe
            {
                Id = RecipeIdToRate,
                Title = "RecipeToRate",
                CookTime = TimeSpan.FromDays(0.03),
                Description = "Desc4",
                UserId = UserAId,
                Ingredients = new List<IngredientInRecipe>
                {
                    new IngredientInRecipe
                    {
                        IngredientId = IngredientBId,
                        Quantity = 0.5,
                        Units = QuantityUnit.Kilograms
                    }
                },
                Ratings = new List<Rating>
                {
                    new Rating
                    {
                        Value = 4,
                        UserId = UserBId
                    }
                }
            });

        context.SaveChanges();

        context.ChangeTracker.Clear();

        return (context, connection);
    }

    public static void Destroy(ApplicationDbContext context, SqliteConnection connection)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
        connection.Dispose();
    }

}
