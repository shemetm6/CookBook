namespace CookBook.Models;

public class Ingredient
{
    public int Id { get; set; }
    public required string Name { get; set; }
    // Если это список рецептов, в которых задействован ингредиент, то почему он хранит элементы типа IngredientInRecipe, а не Recipe?
    // Потому что он и так содержит информацию о рецепте? Но точно ли нам нужны данные о граммовках в рецептах?
    // Как будто бы будет достаточно списка рецептов. Я что-то упускаю?
    public List<IngredientInRecipe> Recipes { get; set; } = new();
}
