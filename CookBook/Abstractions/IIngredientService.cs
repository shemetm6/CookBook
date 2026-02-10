using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IIngredientService
{
    Task<int> AddIngredientAsync(CreateIngredientDto dto);
    Task<ListOfIngredients> GetIngredientsAsync();
    Task<IngredientVm> GetIngredientAsync(int id);
}
