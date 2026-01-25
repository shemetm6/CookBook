using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IIngredientService
{
    int AddIngredient(CreateIngredientDto dto);
    ListOfIngredients GetIngredients();
    IngredientVm GetIngredient(int id);
}
