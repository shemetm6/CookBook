using CookBook.Contracts;
using CookBook.Abstractions;
using CookBook.Models;
using CookBook.Exceptions;
using AutoMapper;
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

    public int AddRecipe(CreateRecipeDto dto, int userId)
    {
        ThrowIfIngredientsNotExist(dto.Ingredients);

        var userExists = _applicationDbContext.Users.Any(u => u.Id == userId);

        if (!userExists)
            throw new UserNotFoundException(userId);

        var recipe = _mapper.Map<Recipe>(dto);

        recipe.UserId = userId;
        
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

    public void UpdateRecipe(int recipeId, UpdateRecipeDto dto, int userId)
    {
        var recipeToUpdate = _applicationDbContext.Recipes
            .Include(r => r.Ingredients) 
            .ThenInclude(ir => ir.Ingredient)
            .FirstOrDefault(r => r.Id == recipeId && r.UserId == userId);

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

    public void DeleteRecipe(int id, int userId)
    {
        var deletedRecipesCount = _applicationDbContext.Recipes
            .Where(r => r.Id == id && r.UserId == userId)
            .ExecuteDelete();

        if(deletedRecipesCount == 0)
            throw new RecipeNotFoundException(id);
    }

    public RecipeVm GetRecipe(int id)
    {
        var recipe = _applicationDbContext.Recipes
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Ingredients)
            .ThenInclude(ir => ir.Ingredient)
            .Include(r => r.Ratings)
            .FirstOrDefault(r => r.Id == id);

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        return _mapper.Map<RecipeVm>(recipe);
    }
    
    public ListOfRecipes GetRecipes()
        => _mapper.Map<ListOfRecipes>(_applicationDbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ratings)
            .OrderBy(r => r.Id)
            .ToList());

    public void RateRecipe(int id, RateRecipeDto dto, int userId)
    {
        var recipe = _applicationDbContext.Recipes
            .Include(r => r.Ratings)
            .FirstOrDefault(r => r.Id == id);

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        var existingRating = recipe.Ratings.FirstOrDefault(rating => rating.UserId == userId);

        if (existingRating is not null)
        {
            existingRating.Value = dto.Value;
            _applicationDbContext.SaveChanges();
            return;
        }

        var rating = _mapper.Map<Rating>(dto);
        rating.RecipeId = recipe.Id;
        rating.UserId = userId;

        recipe.Ratings.Add(rating);

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
