using AutoMapper;
using CookBook.Abstractions;
using static CookBook.Contracts.Recipe;

namespace CookBook.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IIngredientInRecipeRepository _ingredientInRecipeRepository;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IIngredientInRecipeService _ingredientInRecipeService;
    private readonly IMapper _mapper;

    public RecipeService(
        IRecipeRepository recipeRepository,
        IIngredientInRecipeRepository ingredientInRecipeRepository,
        IIngredientRepository ingredientRepository,
        IIngredientInRecipeService ingredientInRecipeService,
        IMapper mapper
    )
    {
        _recipeRepository = recipeRepository;
        _ingredientInRecipeRepository = ingredientInRecipeRepository;
        _ingredientRepository = ingredientRepository;
        _ingredientInRecipeService = ingredientInRecipeService;
        _mapper = mapper;
    }

    public int AddRecipe(CreateRecipeDto dto)
    {
        EnsureIngredientsExist(dto.Ingredients);

        var recipeId = _recipeRepository.AddRecipe(dto.Title, dto.CookTime, dto.TimeUnit, dto.Description);

        _ingredientInRecipeService.AttachAllIngredientsToRecipe(recipeId, dto.Ingredients);

        return recipeId;
    }

    public void UpdateRecipe(int recipeId, UpdateRecipeDto dto)
    {
        EnsureIngredientsExist(dto.Ingredients);

        _recipeRepository.UpdateRecipe(recipeId, dto.Title, dto.CookTime, dto.TimeUnit, dto.Description);

        _ingredientInRecipeService.ReplaceAllIngredientsInRecipe(recipeId, dto.Ingredients);
    }

    public void DeleteRecipe(int id)
    {
        var recipe = _recipeRepository.GetRecipe(id);

        foreach (var ingredient in recipe.Ingredients.ToList())
        {
            _ingredientInRecipeRepository.RemoveIngredientFromRecipe(ingredient);
        }

        _recipeRepository.DeleteRecipe(id);
    }

    public RecipeVm GetRecipe(int id) 
        => _mapper.Map<RecipeVm>(_recipeRepository.GetRecipe(id));

    public ListOfRecipes GetRecipes() 
        => _mapper.Map<ListOfRecipes>(_recipeRepository.GetRecipes());
    
    public void RateRecipe(int id, RateRecipeDto dto)
    {
        _recipeRepository.RateRecipe(id, dto.Raiting);
    }

    private void EnsureIngredientsExist(IEnumerable<IngredientInRecipeCreateVm> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            _ingredientRepository.GetIngredient(ingredient.IngredientId);
        }
    }
}
