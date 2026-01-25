using AutoMapper;
using CookBook.Abstractions;
using CookBook.Exceptions;
using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using CookBook.Contracts;

namespace CookBook.Services;

public class IngredientService : IIngredientService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public IngredientService(
        IApplicationDbContext applicationDbContext, 
        IMapper mapper
        )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public int AddIngredient(CreateIngredientDto dto)
    {
        var ingredient = _mapper.Map<Ingredient>(dto);

        _applicationDbContext.Ingredients.Add(ingredient);

        _applicationDbContext.SaveChanges();

        return ingredient.Id;
    }

    public ListOfIngredients GetIngredients() 
        => _mapper.Map<ListOfIngredients>(_applicationDbContext.Ingredients.AsNoTracking().ToList());

    public IngredientVm GetIngredient(int id)
    {
        var ingredient = _applicationDbContext.Ingredients.AsNoTracking().FirstOrDefault(i => i.Id == id);

        return ingredient is null ? throw new IngredientNotFoundException(id) : _mapper.Map<IngredientVm>(ingredient);
    }
}
