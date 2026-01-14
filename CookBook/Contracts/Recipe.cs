using CookBook.Enums;

namespace CookBook.Contracts;

public class Recipe
{
    public record RecipeVm(
        int Id, 
        string Title, 
        TimeSpan CookTime, 
        List<IngredientsInRecipeVm> Ingredients, 
        string Description, 
        double? AverageRating);

    public record IngredientsInRecipeVm(
        int IngredientId, 
        string IngredientTitle,
        double Quantity, 
        QuantityUnit? Units);

    public record IngredientInRecipeCreateVm(
        int IngredientId,
        double Quantity,
        QuantityUnit? Units);

    public record RecipesListVm(int Id, string Title, double? AverageRating);

    public record ListOfRecipes(List<RecipesListVm> Recipes);

    public record CreateRecipeDto(
        string Title, 
        double CookTime, 
        TimeUnit TimeUnit, 
        List<IngredientInRecipeCreateVm> Ingredients, 
        string Description);

    public record UpdateRecipeDto( 
        string Title, 
        double CookTime, 
        TimeUnit TimeUnit, 
        List<IngredientInRecipeCreateVm> Ingredients, 
        string Description);

    public record RateRecipeDto(Raiting Raiting);

}
