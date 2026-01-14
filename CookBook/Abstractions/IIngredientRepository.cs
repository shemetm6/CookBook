using static CookBook.Contracts.Ingredient;

namespace CookBook.Abstractions;

public interface IIngredientRepository
{
    int AddIngredient(CreateIngredientDto dto);
    Models.Ingredient GetIngredient(int id); //(!) пока не использую здесь Vm. Подумой, что должно передаваться во внутренней логике - объект или объектvm. А лучше сделай get метод
    ListOfIngredients GetIngredients();
}
