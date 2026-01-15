using static CookBook.Contracts.Ingredient;

namespace CookBook.Abstractions;

public interface IIngredientRepository
{
    int AddIngredient(CreateIngredientDto dto);
    // Пока не использую здесь Vm т.к. метод используется только во внутренней логике т.е. для работы с оригинальной моделью, а не с dto.
    Models.Ingredient GetIngredient(int id); 
    ListOfIngredients GetIngredients();
}
