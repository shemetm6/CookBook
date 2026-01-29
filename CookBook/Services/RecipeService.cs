using AutoMapper;
using CookBook.Abstractions;
using CookBook.Contracts;
using CookBook.Exceptions;
using CookBook.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services;

public class RecipeService : IRecipeService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly ITimeConverter _timeConverter;

    public RecipeService(
        IApplicationDbContext applicationDbContext,
        IMapper mapper,
        ITimeConverter timeConverter
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _timeConverter = timeConverter;
    }

    public int AddRecipe(CreateRecipeDto dto)
    {
        ThrowIfIngredientsNotExist(dto.Ingredients);

        var recipe = _mapper.Map<Recipe>(dto);

        recipe.CookTime = _timeConverter.Convert(dto.CookTime, dto.TimeUnit);

        foreach (var ingredientInRecipe in recipe.Ingredients)
        {
            var existingIngredient = _applicationDbContext.Ingredients
                .First(i => i.Id == ingredientInRecipe.IngredientId);

            ingredientInRecipe.Recipe = recipe; 
            ingredientInRecipe.Ingredient = existingIngredient;
        }

        _applicationDbContext.Recipes.Add(recipe);
        _applicationDbContext.SaveChanges();

        return recipe.Id;
    }

    public void UpdateRecipe(int recipeId, UpdateRecipeDto dto)
    {
        var recipeToUpdate = _applicationDbContext.Recipes
            .Include(r => r.Ingredients) 
            .ThenInclude(ir => ir.Ingredient)
            .FirstOrDefault(r => r.Id == recipeId);

        if (recipeToUpdate is null)
            throw new RecipeNotFoundException(recipeId);

        ThrowIfIngredientsNotExist(dto.Ingredients);

        foreach (var ingredientInRecipe in recipeToUpdate.Ingredients.ToList())
        {
            recipeToUpdate.Ingredients.Remove(ingredientInRecipe);
        }

        foreach (var ingredientInRecipeVm in dto.Ingredients)
        {
            var ingredientInRecipe = _mapper.Map<IngredientInRecipe>(ingredientInRecipeVm);

            var existingIngredient = _applicationDbContext.Ingredients
                .First(i => i.Id == ingredientInRecipe.IngredientId);

            ingredientInRecipe.Recipe = recipeToUpdate;
            ingredientInRecipe.Ingredient = existingIngredient;

            recipeToUpdate.Ingredients.Add(ingredientInRecipe);
        }

        recipeToUpdate.Title = dto.Title;
        recipeToUpdate.Description = dto.Description;
        recipeToUpdate.CookTime = _timeConverter.Convert(dto.CookTime, dto.TimeUnit);

        _applicationDbContext.SaveChanges();
    }

    public void DeleteRecipe(int id)
    {
        var deletedRecipesCount = _applicationDbContext.Recipes
            .Where(r => r.Id == id)
            .ExecuteDelete();

        if(deletedRecipesCount == 0)
            throw new RecipeNotFoundException(id);
    }

    public RecipeVm GetRecipe(int id)
    {
        var recipe = _applicationDbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .ThenInclude(ir => ir.Ingredient)
            .FirstOrDefault(r => r.Id == id);

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        return _mapper.Map<RecipeVm>(recipe);
    }
    
    public ListOfRecipes GetRecipes()
        => _mapper.Map<ListOfRecipes>(_applicationDbContext.Recipes
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .ToList());

    public void RateRecipe(int id, RateRecipeDto dto)
    {
        var recipe = _applicationDbContext.Recipes.FirstOrDefault(r => r.Id == id);

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        recipe.Ratings.Add((int)dto.Rating);

        _applicationDbContext.SaveChanges();
    }

    private void ThrowIfIngredientsNotExist(IEnumerable<IngredientInRecipeCreateVm> ingredients)
    {
        var dtoIngredientIds = ingredients
            .Select(i => i.IngredientId)
            .ToList();

        var existingIngredientIds = _applicationDbContext.Ingredients
            .Where(i => dtoIngredientIds.Contains(i.Id))
            .Select(i => i.Id)
            .ToList();

        var invalidIngredientIds = dtoIngredientIds
            .Except(existingIngredientIds)
            .ToList();

        if (invalidIngredientIds.Count != 0)
            throw new IngredientNotFoundException(invalidIngredientIds);
    }
}
