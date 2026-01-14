using AutoMapper;
using CookBook.Abstractions;
using CookBook.Exceptions;
using static CookBook.Contracts.Ingredient;

namespace CookBook.Services;

public class IngredientRepository : IIngredientRepository
{
    private readonly List<Models.Ingredient> _ingredients = [];
    private readonly IMapper _mapper;

    public IngredientRepository(IMapper mapper) => _mapper = mapper;

    public int AddIngredient(CreateIngredientDto dto)
    {
        var ingredientId = _ingredients.Count;

        _ingredients.Add(new Models.Ingredient()
        {
            Id = ingredientId,
            Name = dto.Name,
        });

        return ingredientId;
    }

    // Если у меня появится гет запрос для конкретного ингредиента, то возвращаемое значение будет IngredientVm?
    // Меня смущает, что сам объект ингредиента используется во внутренней логике (IngredientInRecipeRepository например)
    // А изходя из этого он как будто бы должен всегда быть изначальным объектом, а не view model
    public Models.Ingredient GetIngredient(int id)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == id);

        return ingredient ?? throw new IngredientNotAllowedException(id, _ingredients);
    }

    public ListOfIngredients GetIngredients() => _mapper.Map<ListOfIngredients>(_ingredients);
}
