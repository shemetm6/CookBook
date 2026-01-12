using CookBook.Models;

namespace CookBook.Contracts;

public class Ingredient
{
    public record IngredientListVm(int Id, string Name, List<RecipeInIngredientVm> Recipes);
    public record RecipeInIngredientVm(int RecipeId, string RecipeTitle);
    public record ListOfIngredients(List<IngredientListVm> Ingredients);
    public record CreateIngredientDto(string Name);

}
