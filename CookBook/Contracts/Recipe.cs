using CookBook.Enums;

namespace CookBook.Contracts;

public record RecipeVm(
    int Id,
    string Title,
    string UserLogin,
    TimeSpan CookTime,
    IReadOnlyCollection<IngredientInRecipeVm> Ingredients,
    string Description,
    double? AverageRating
    );
public record IngredientInRecipeVm(
    int IngredientId,
    string IngredientName,
    double Quantity,
    QuantityUnit Units
    );
public record IngredientInRecipeCreateVm(
    int IngredientId,
    double Quantity,
    QuantityUnit Units
    );

public record RecipeInListVm(int Id, string Title, TimeSpan CookTime, double? AverageRating, string Author);
public record ListOfRecipes(IReadOnlyList<RecipeInListVm> Recipes);

public record CreateRecipeDto(
    string Title,
    double CookTime,
    TimeUnit TimeUnit,
    IReadOnlyCollection<IngredientInRecipeCreateVm> Ingredients,
    string Description
    );
public record UpdateRecipeDto(
    string Title,
    double CookTime,
    TimeUnit TimeUnit,
    IReadOnlyCollection<IngredientInRecipeCreateVm> Ingredients,
    string Description
    );

public record RateRecipeDto(int Value);
