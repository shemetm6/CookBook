using AutoMapper;
using CookBook.Abstractions;
using static CookBook.Contracts.Ingredient;

namespace CookBook.Services;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;

    public IngredientService(
        IIngredientRepository ingredientRepository,
        IMapper mapper
    )
    {
        _ingredientRepository = ingredientRepository;
        _mapper = mapper;
    }

    public int AddIngredient(CreateIngredientDto dto) 
        => _ingredientRepository.AddIngredient(dto.Name);

    public ListOfIngredients GetIngredients()
        => _mapper.Map<ListOfIngredients>(_ingredientRepository.GetIngredients());

    public IngredientVm GetIngredient(int id)
        => _mapper.Map<IngredientVm>(_ingredientRepository.GetIngredient(id));
}