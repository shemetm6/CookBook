namespace CookBook.Contracts;

public record IngredientVm(int Id, string Name, IReadOnlyCollection<RecipeInIngredientVm> Recipes);
public record IngredientInListVm(int Id, string Name);
public record RecipeInIngredientVm(int RecipeId, string RecipeTitle);
public record ListOfIngredients(IReadOnlyList<IngredientInListVm> Ingredients);
public record CreateIngredientDto(string Name);
