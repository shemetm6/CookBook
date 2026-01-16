using static CookBook.Contracts.Ingredient;

namespace CookBook.Abstractions;

public interface IIngredientService
{
    int AddIngredient(CreateIngredientDto dto);
    ListOfIngredients GetIngredients();
    IngredientVm GetIngredient(int id);
}
