using CookBook.Contracts;
using CookBook.Abstractions;
using CookBook.Models;
using CookBook.Exceptions;
using CookBook.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace CookBook.Services;

public class RecipeService : IRecipeService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly ITimeConverter _timeConverter;
    private readonly ILogger<RecipeService> _logger;

    public RecipeService(
        IApplicationDbContext applicationDbContext,
        IMapper mapper,
        ITimeConverter timeConverter,
        ILogger<RecipeService> logger
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _timeConverter = timeConverter;
        _logger = logger;
    }

    public async Task<int> AddRecipeAsync(CreateRecipeDto dto, int userId)
    {
        var existingIngredients = await GetIngredientsDictionaryOrThrowAsync(dto.Ingredients);

        var userExists = await _applicationDbContext.Users.AnyAsync(u => u.Id == userId);

        if (!userExists)
        {
            // Если мы не нашли юзера с таким id, то это проблема на стороне приложения
            // т.к. userId вытаскивается из HttpContext.
            // Исходя из этого как будто бы можно убрать выбрасывание исключения
            // Но!
            // Клод подкинул мыслю, что разные слои приложения не должны доверять друг другу
            // Сегодня эти методы вызываются из контроллера, где проверка есть,
            // а завтра их вызовут из другого места и напишут рандомный id
            // Но!
            // Тогда наверное и в остальных методах тоже надо делать проверку
            // Т.е. для однородности надо либо убрать эту проверку либо добавить такую же в Delete и Update
            // Как быть?
            _logger.LogError("Couldn't get user with {UserId}. Not found.", userId);
            throw new UserNotFoundException(userId);
        }

        var recipe = _mapper.Map<Recipe>(dto);

        recipe.UserId = userId;
        recipe.CookTime = _timeConverter.Convert(dto.CookTime, dto.TimeUnit);

        foreach (var ingredientInRecipe in recipe.Ingredients)
        {
            ingredientInRecipe.Recipe = recipe;
            ingredientInRecipe.Ingredient = existingIngredients[ingredientInRecipe.IngredientId];
        }

        await _applicationDbContext.Recipes.AddAsync(recipe);
        await _applicationDbContext.SaveChangesAsync();

        _logger.LogInformation("Successfully added recipe with {Id}", recipe.Id);

        return recipe.Id;
    }

    public async Task UpdateRecipeAsync(int recipeId, UpdateRecipeDto dto, int userId)
    {
        var recipeToUpdate = await _applicationDbContext.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId && r.UserId == userId);

        if (recipeToUpdate is null)
        {
            _logger.LogError("Couldn't get recipe with {RecipeId}. Not found.", recipeId);
            throw new RecipeNotFoundException(recipeId);
        }
            
        var existingIngredients = await GetIngredientsDictionaryOrThrowAsync(dto.Ingredients);

        recipeToUpdate.Ingredients.Clear();

        foreach (var ingredientInRecipeCreateVm in dto.Ingredients)
        {
            var ingredientInRecipe = _mapper.Map<IngredientInRecipe>(ingredientInRecipeCreateVm);

            ingredientInRecipe.Recipe = recipeToUpdate;
            ingredientInRecipe.Ingredient = existingIngredients[ingredientInRecipe.IngredientId];

            recipeToUpdate.Ingredients.Add(ingredientInRecipe);
        }

        recipeToUpdate.Title = dto.Title;
        recipeToUpdate.Description = dto.Description;
        recipeToUpdate.CookTime = _timeConverter.Convert(dto.CookTime, dto.TimeUnit);

        _logger.LogInformation("Successfully updated recipe with {Id}", recipeToUpdate.Id);

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteRecipeAsync(int id, int userId)
    {
        var deletedRecipesCount = await _applicationDbContext.Recipes
            .Where(r => r.Id == id && r.UserId == userId)
            .ExecuteDeleteAsync();

        if (deletedRecipesCount == 0)
        {
            _logger.LogError("Couldn't get recipe with {Id}. Not found.", id);
            throw new RecipeNotFoundException(id);
        }

        _logger.LogInformation("Successfully deleted recipe with {Id}", id);
    }

    public async Task<RecipeVm> GetRecipeAsync(int id)
    {
        var recipe = await _applicationDbContext.Recipes
            .AsNoTracking()
            .Where(r => r.Id == id)
            .ProjectTo<RecipeVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (recipe is null)
        {
            _logger.LogError("Couldn't get recipe with {Id}. Not found.", id);
            throw new RecipeNotFoundException(id);
        }

        return recipe;
    }

    public async Task<ListOfRecipes> GetRecipesAsync(
        string? title,
        double? minRating,
        string? author,
        RecipeSortBy? sortBy,
        bool? descending
        )
    {
        var query = _applicationDbContext.Recipes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(recipe => recipe.Title.ToLower().Contains(title.Trim().ToLower()));

        if (minRating is not null)
            query = query.Where(recipe => recipe.Ratings.Average(rating => rating.Value) >= minRating);

        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(recipe => recipe.User.Login.Trim().ToLower().Contains(author.Trim().ToLower()));

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

        var recipes = await query
            .Select(r => new RecipeInListVm(
            r.Id,
            r.Title,
            r.CookTime,
            r.Ratings.Average(rating => rating.Value),
            r.User.Login))
            .ToListAsync();

        return _mapper.Map<ListOfRecipes>(recipes);
    }

    public async Task RateRecipeAsync(int id, RateRecipeDto dto, int userId)
    {
        var recipe = await _applicationDbContext.Recipes
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe is null)
        {
            _logger.LogError("Couldn't get recipe with {Id}. Not found.", id);
            throw new RecipeNotFoundException(id);
        }

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

        _logger.LogInformation("Successfully rated recipe with {Id}", id);
    }

    // Название просто пиздец, но лучше пока не придумал
    private async Task<Dictionary<int, Ingredient>> GetIngredientsDictionaryOrThrowAsync(
        IEnumerable<IngredientInRecipeCreateVm> ingredients
        )
    {
        _logger.LogDebug("Obtaining ingredients from the database: {@Ingredients}", ingredients);
        var dtoIngredientIds = ingredients
            .Select(i => i.IngredientId)
            .ToList();

        var existingIngredients = await _applicationDbContext.Ingredients
            .Where(i => dtoIngredientIds.Contains(i.Id))
            .ToDictionaryAsync(i => i.Id);

        var invalidIngredientIds = dtoIngredientIds
            .Except(existingIngredients.Keys)
            .ToList();

        if (invalidIngredientIds.Count != 0)
        {
            _logger.LogError("Couldn't get ingredients with {InvalidIngredientIds}. Not found.", string.Join(", ", invalidIngredientIds));
            throw new IngredientNotFoundException(invalidIngredientIds);
        }

        return existingIngredients;
    }
}
