namespace CookBook.Contracts;

public class Ingredient
{
    // Пока IngredientVm почти не используется т.к. у меня нет метода get запроса на конретный ингредиент.
    // Сделал его исключительно для маппинга и то не уверен нужен ли он там. Вцелом уже плохо вывожу, что происходит в приложении.
    public record IngredientVm(int Id, string Name, List<RecipeInIngredientVm> Recipes); 
    public record IngredientInListVm(int Id, string Name, List<RecipeInIngredientVm> Recipes);
    public record RecipeInIngredientVm(int RecipeId, string RecipeTitle);
    public record ListOfIngredients(IReadOnlyList<IngredientInListVm> Ingredients);
    public record CreateIngredientDto(string Name);
}
