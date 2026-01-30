using CookBook.Enums;

namespace CookBook.Contracts;

public record RecipeVm(
    int Id,
    string Title,
    string UserLogin,
    TimeSpan CookTime,
    List<IngredientInRecipeVm> Ingredients,
    string Description,
    double? AverageRating
    );
public record IngredientInRecipeVm(
    int IngredientId,
    string IngredientName,
    double Quantity,
    QuantityUnit? Units
    );
public record IngredientInRecipeCreateVm(
    int IngredientId,
    double Quantity,
    QuantityUnit? Units
    );

public record RecipeInListVm(int Id, string Title, double? AverageRating);
public record ListOfRecipes(List<RecipeInListVm> Recipes);

public record CreateRecipeDto(
    int UserId,
    string Title,
    double CookTime,
    TimeUnit TimeUnit,
    List<IngredientInRecipeCreateVm> Ingredients,
    string Description
    );
public record UpdateRecipeDto(
    string Title,
    double CookTime,
    TimeUnit TimeUnit,
    List<IngredientInRecipeCreateVm> Ingredients,
    string Description
    );

public record RateRecipeDto(Rating Rating);
