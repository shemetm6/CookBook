using CookBook.Enums;
using System.Text.Json.Serialization;

namespace CookBook.Models;

public class IngredientInRecipe
{
    public int IgredientId { get; set; }
    public int RecipeId { get; set; }
    public double Quantity { get; set; }
    public QuantityUnit Units { get; set; }
    // Я намудил с зависимостями одних репозиториев от других и приложение зациклилось
    // В рабочее состояние смог вернуть только отказавшись от хранения рецепта и ингридиента в этой модели
    // Надеюсь потом смогу допинать
    //public Recipe Recipe { get; set; }
    //public Ingredient Ingredient { get; set; }
}
