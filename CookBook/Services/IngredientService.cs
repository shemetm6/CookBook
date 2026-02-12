using CookBook.Contracts;
using CookBook.Abstractions;
using CookBook.Models;
using CookBook.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace CookBook.Services;

public class IngredientService : IIngredientService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<IngredientService> _logger;

    public IngredientService(
        IApplicationDbContext applicationDbContext, 
        IMapper mapper,
        ILogger<IngredientService> logger
        )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> AddIngredientAsync(CreateIngredientDto dto)
    {
        var ingredient = _mapper.Map<Ingredient>(dto);

        await _applicationDbContext.Ingredients.AddAsync(ingredient);

        await _applicationDbContext.SaveChangesAsync();

        _logger.LogInformation("Successfully added ingredient with {Id}", ingredient.Id);

        return ingredient.Id;
    }

    public async Task<ListOfIngredients> GetIngredientsAsync()
    {
        var ingredients = await _applicationDbContext.Ingredients
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<ListOfIngredients>(ingredients);
    }

    public async Task<IngredientVm> GetIngredientAsync(int id)
    {
        var ingredient = await _applicationDbContext.Ingredients
            .AsNoTracking()
            .Where(i => i.Id == id)
            .ProjectTo<IngredientVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (ingredient is null)
        {
            _logger.LogError("Couldn't get ingredient with {Id}. Not found.", id);
            throw new IngredientNotFoundException(id);
        }

        return ingredient;
    }
}
