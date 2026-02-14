using CookBook.Contracts;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CookBook.Tests;

public class RecipeServiceTests : TestServiceBase
{
    private readonly RecipeService _sut;

    public RecipeServiceTests() : base()
    {
        var logger = new NullLogger<RecipeService>();
        _sut = new(Context, Mapper, TimeConverter, logger);
    }

    [Fact]
    public async Task AddRecipeAsync_WhenCorrectInput_SuccessfullyCreated()
    {
        // Arrange
        var dto = new CreateRecipeDto(
            Title: "Recipe1",
            CookTime: 4,
            TimeUnit: TimeUnit.Minutes,
            Ingredients: new List<IngredientInRecipeCreateVm>
            {
                new IngredientInRecipeCreateVm(
                    IngredientId: FakeApplicationDbContextFactory.IngredientAId,
                    Quantity: 4,
                    Units: QuantityUnit.Grams)
            },
            Description: "Desc1"
            );

        // Act
        var result = await _sut.AddRecipeAsync(
            dto: dto,
            userId: FakeApplicationDbContextFactory.UserAId
            );

        // Assert
        var created = await Context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == result);

        Assert.NotNull(created);
        Assert.Equal("Recipe1", created.Title);
        Assert.Equal(4, created.CookTime.TotalMinutes);
        Assert.Equal("Desc1", created.Description);

        Assert.Single(created.Ingredients);
        var ingredient = created.Ingredients.First();

        Assert.Equal(FakeApplicationDbContextFactory.IngredientAId, ingredient.IngredientId);
        Assert.Equal(4, ingredient.Quantity);
        Assert.Equal(QuantityUnit.Grams, ingredient.Units);
    }

    [Fact]
    public async Task AddRecipeAsync_WhenUserNotExists_ThrowsUserNotFoundException()
    {
        // Arrange
        var dto = new CreateRecipeDto(
            Title: "Recipe1",
            CookTime: 4,
            TimeUnit: TimeUnit.Minutes,
            Ingredients: new List<IngredientInRecipeCreateVm>
            {
                new IngredientInRecipeCreateVm(
                    IngredientId: FakeApplicationDbContextFactory.IngredientAId,
                    Quantity: 4,
                    Units: QuantityUnit.Grams)
            },
            Description: "Desc1"
            );

        // Act
        var act = async () => await _sut.AddRecipeAsync(
            dto: dto,
            userId: 999999999
            );

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(act);
    }

    [Fact]
    public async Task DeleteRecipeAsync_WhenRecipeExists_RemoveSuccessfully()
    {
        // Arrange

        // Act
        await _sut.DeleteRecipeAsync(
            id: FakeApplicationDbContextFactory.RecipeIdToDelete,
            userId: FakeApplicationDbContextFactory.UserAId
            );

        // Assert
        var deleted = await Context.Recipes.FindAsync(FakeApplicationDbContextFactory.RecipeIdToDelete);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteRecipeAsync_WhenRecipeNotExists_ThrowsRecipeNotFoundException()
    {
        // Arrange

        // Act
        var act = async () => await _sut.DeleteRecipeAsync(
            id: 999999999,
            userId: FakeApplicationDbContextFactory.UserAId
            );

        // Assert
        await Assert.ThrowsAsync<RecipeNotFoundException>(act);
    }

    [Fact]
    public async Task UpdateRecipeAsync_WhenCorrectInput_UpdateSuccessfully()
    {
        // Arrange
        var dto = new UpdateRecipeDto(
        Title: "UpdatedRecipe",
        CookTime: 5,
        TimeUnit: TimeUnit.Minutes,
        Ingredients: new List<IngredientInRecipeCreateVm>
        {
            new IngredientInRecipeCreateVm(
                IngredientId: FakeApplicationDbContextFactory.IngredientAId,
                Quantity: 10,
                Units: QuantityUnit.Cups
            )
        },
        Description: "Updated description"
        );

        // Act
        await _sut.UpdateRecipeAsync(
            recipeId: FakeApplicationDbContextFactory.RecipeIdToUpdate,
            dto: dto,
            userId: FakeApplicationDbContextFactory.UserBId
            );

        // Assert
        var updated = await Context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == FakeApplicationDbContextFactory.RecipeIdToUpdate);

        Assert.NotNull(updated);
        Assert.Equal("UpdatedRecipe", updated.Title);
        Assert.Equal(5, updated.CookTime.TotalMinutes);
        Assert.Equal("Updated description", updated.Description);

        Assert.Single(updated.Ingredients);
        var ingredient = updated.Ingredients.First();

        Assert.Equal(FakeApplicationDbContextFactory.IngredientAId, ingredient.IngredientId);
        Assert.Equal(10, ingredient.Quantity);
        Assert.Equal(QuantityUnit.Cups, ingredient.Units);
    }

    [Fact]
    public async Task UpdateRecipeAsync_WhenIncorrectInput_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var dto = new UpdateRecipeDto(
        Title: "UpdatedRecipe",
        CookTime: 5,
        TimeUnit: TimeUnit.Minutes,
        Ingredients: new List<IngredientInRecipeCreateVm>
        {
            new IngredientInRecipeCreateVm(
                IngredientId: FakeApplicationDbContextFactory.IngredientAId,
                Quantity: 10,
                Units: (QuantityUnit)999999999
            )
        },
        Description: "Updated description"
        );

        // Act
        var act = async () => await _sut.UpdateRecipeAsync(
                recipeId: FakeApplicationDbContextFactory.RecipeIdToUpdate,
                dto: dto,
                userId: FakeApplicationDbContextFactory.UserBId
                );

        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(act);
    }

    [Fact]
    public async Task RateRecipeAsync_WhenCorrectInputAndRecipeExists_RateSuccessfully()
    {
        // Arrange
        var dto = new RateRecipeDto(5);

        // Act
        await _sut.RateRecipeAsync(
            id: FakeApplicationDbContextFactory.RecipeIdToRate,
            dto: dto,
            userId: FakeApplicationDbContextFactory.UserAId
            );

        // Assert
        var ratedRecipe = await Context.Recipes
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == FakeApplicationDbContextFactory.RecipeIdToRate);

        Assert.NotNull(ratedRecipe);

        var rating = ratedRecipe.Ratings.First(rating => rating.Value == 5);
        Assert.Equal(5, rating.Value);
    }

    // Если я хочу написать тест, в котором будет передан некорретный Value рейтинга,
    // а за валидацию этого значения отвечает FluentValidation,
    // то мне нужно делать отдельный RecipesControllerTests.cs,
    // т.к. выброс исключения (или что там у FluentValidation) произойдет на уровне контроллера?
    // Ну или RateRecipeValidatorTests.cs, если пойти глубже?
}
