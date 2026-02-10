using CookBook.Contracts;
using CookBook.Abstractions;
using CookBook.Models;
using CookBook.Exceptions;
using CookBook.Enums;
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

    public async Task<int> AddRecipeAsync(CreateRecipeDto dto, int userId)
    {
        await ThrowIfIngredientsNotExistAsync(dto.Ingredients);

        var userExists = await _applicationDbContext.Users.AnyAsync(u => u.Id == userId);

        if (!userExists)
            throw new UserNotFoundException(userId);

        var recipe = _mapper.Map<Recipe>(dto);

        recipe.UserId = userId;

        recipe.CookTime = _timeConverter.Convert(dto.CookTime, dto.TimeUnit);

        foreach (var ingredientInRecipe in recipe.Ingredients)
        {
            var existingIngredient = await _applicationDbContext.Ingredients
                .FirstAsync(i => i.Id == ingredientInRecipe.IngredientId);

            ingredientInRecipe.Recipe = recipe;
            ingredientInRecipe.Ingredient = existingIngredient;
        }

        await _applicationDbContext.Recipes.AddAsync(recipe);
        await _applicationDbContext.SaveChangesAsync();

        return recipe.Id;
    }

    public async Task UpdateRecipeAsync(int recipeId, UpdateRecipeDto dto, int userId)
    {
        var recipeToUpdate = await _applicationDbContext.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ir => ir.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == recipeId && r.UserId == userId);

        if (recipeToUpdate is null)
            throw new RecipeNotFoundException(recipeId);

        await ThrowIfIngredientsNotExistAsync(dto.Ingredients);

        foreach (var ingredientInRecipe in recipeToUpdate.Ingredients.ToList())
        {
            recipeToUpdate.Ingredients.Remove(ingredientInRecipe);
        }

        foreach (var ingredientInRecipeVm in dto.Ingredients)
        {
            var ingredientInRecipe = _mapper.Map<IngredientInRecipe>(ingredientInRecipeVm);

            var existingIngredient = await _applicationDbContext.Ingredients
                .FirstAsync(i => i.Id == ingredientInRecipe.IngredientId);

            ingredientInRecipe.Recipe = recipeToUpdate;
            ingredientInRecipe.Ingredient = existingIngredient;

            recipeToUpdate.Ingredients.Add(ingredientInRecipe);
        }

        recipeToUpdate.Title = dto.Title;
        recipeToUpdate.Description = dto.Description;
        recipeToUpdate.CookTime = _timeConverter.Convert(dto.CookTime, dto.TimeUnit);

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteRecipeAsync(int id, int userId)
    {
        var deletedRecipesCount = await _applicationDbContext.Recipes
            .Where(r => r.Id == id && r.UserId == userId)
            .ExecuteDeleteAsync();

        if (deletedRecipesCount == 0)
            throw new RecipeNotFoundException(id);
    }

    public async Task<RecipeVm> GetRecipeAsync(int id)
    {
        var recipe = await _applicationDbContext.Recipes
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Ingredients)
            .ThenInclude(ir => ir.Ingredient)
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        return _mapper.Map<RecipeVm>(recipe);
    }

    public async Task<ListOfRecipes> GetRecipesAsync(
        string? title,
        double? minRating,
        string? author,
        RecipeSortBy? sortBy,
        bool? descending
        )
    {
        var query = _applicationDbContext.Recipes
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Ratings)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(recipe => recipe.Title.ToLower().Contains(title.Trim().ToLower()));

        if (minRating is not null)
            query = query.Where(recipe => recipe.Ratings.Average(rating => rating.Value) >= minRating);

        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(recipe => recipe.User.Login.Trim().ToLower().Contains(author.Trim().ToLower()));

        // По ТЗ сортировка по времени готовки не требовалась, но enum с двумя наименованиями выглядел печально.
        // И я хотел посмотреть нормально ли будет сортироваться TimeSpan.
        query = sortBy switch
        {
            RecipeSortBy.Title => descending == true
            ? query.OrderByDescending(recipe => recipe.Title)
            : query.OrderBy(recipe => recipe.Title),

            RecipeSortBy.Rating => descending == true 
            ? query.OrderByDescending(recipe => recipe.Ratings.Average(rating => rating.Value)) 
            : query.OrderBy(recipe => recipe.Ratings.Average(rating => rating.Value)),

            RecipeSortBy.CookTime => descending == true
            ? query.OrderByDescending(recipe => recipe.CookTime)
            : query.OrderBy(recipe => recipe.CookTime),

            _ => descending == true 
            ? query.OrderByDescending(recipe => recipe.Id)
            : query.OrderBy(recipe => recipe.Id),
        };

        var recipes = await query.ToListAsync();

        return _mapper.Map<ListOfRecipes>(recipes);
    }

    public async Task RateRecipeAsync(int id, RateRecipeDto dto, int userId)
    {
        var recipe = await _applicationDbContext.Recipes
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe is null)
            throw new RecipeNotFoundException(id);

        var existingRating = recipe.Ratings.FirstOrDefault(rating => rating.UserId == userId);

        if (existingRating is not null)
        {
            existingRating.Value = dto.Value;

            await _applicationDbContext.SaveChangesAsync();

            return;
        }

        var rating = _mapper.Map<Rating>(dto);
        rating.RecipeId = recipe.Id;
        rating.UserId = userId;

        recipe.Ratings.Add(rating);

        await _applicationDbContext.SaveChangesAsync();
    }

    private async Task ThrowIfIngredientsNotExistAsync(IEnumerable<IngredientInRecipeCreateVm> ingredients)
    {
        var dtoIngredientIds = ingredients
            .Select(i => i.IngredientId)
            .ToList();

        var existingIngredientIds = await _applicationDbContext.Ingredients
            .Where(i => dtoIngredientIds.Contains(i.Id))
            .Select(i => i.Id)
            .ToListAsync();

        var invalidIngredientIds = dtoIngredientIds
            .Except(existingIngredientIds)
            .ToList();

        if (invalidIngredientIds.Count != 0)
            throw new IngredientNotFoundException(invalidIngredientIds);
    }
}
