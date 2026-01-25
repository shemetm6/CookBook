namespace CookBook.Contracts;

public record IngredientVm(int Id, string Name, List<RecipeInIngredientVm> Recipes);
public record IngredientInListVm(int Id, string Name);
public record RecipeInIngredientVm(int RecipeId, string RecipeTitle);
public record ListOfIngredients(List<IngredientInListVm> Ingredients);
public record CreateIngredientDto(string Name);
