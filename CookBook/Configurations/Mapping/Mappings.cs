using AutoMapper;
using CookBook.Models;
using static CookBook.Contracts.Ingredient;
//using static CookBook.Contracts.Recipe;

namespace CookBook.Configurations.Mapping;

public class IngredientMappingProfile : Profile
{
    public IngredientMappingProfile()
    {
        CreateMap<IngredientInRecipe, RecipeInIngredientVm>()
            .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.RecipeId))
            .ForMember(dest => dest.RecipeTitle, opt => opt.MapFrom(src => src.Recipe!.Title));

        CreateMap<Ingredient, IngredientVm>()
            .ForCtorParam(nameof(IngredientListVm.Recipes), opt => opt.MapFrom(src => src.Recipes));

        CreateMap<Ingredient, IngredientListVm>()
            .ForCtorParam(nameof(IngredientListVm.Recipes), opt => opt.MapFrom(src => src.Recipes));

        CreateMap<IEnumerable<Ingredient>, ListOfIngredients>()
            .ForCtorParam(nameof(ListOfIngredients.Ingredients),
            src => src.MapFrom(src => src.ToList()));

        CreateMap<CreateIngredientDto, Ingredient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Recipes, opt => opt.Ignore());
    }
}

// Я пытался, но жоско запутался.
// Вот в методах репозитория я должен начать принимать dto
// Но тогда мне придется принимать dto еще и в RecipeService т.к. там дублируются методы RecipeRepository
// А в ходе реализации некоторых методов RecipeService вызывается GetRecipe от вышеупомянутого репозитория.
// И этот GetRecipe будет возвращать RecipeVm вместо Recipe
// И дальше я уже не могу нормально приаттачить/удалить ингредиенты из рецепта
// т.к. приаттачить/удалить это методы из IngredientInRecipeRepository, который тоже работает с обычными моделями, а не с контрактами
// И шо мне делать? Переводить IngredientInRecipeRepository и RecipeService на Dto?
// Звучит странно, они же реализуют внутреннюю логику, зачем им эти контракты
/*
public class RecipeMappingProfile : Profile
{
    public RecipeMappingProfile()
    {
        CreateMap<IngredientInRecipe, IngredientsInRecipeVm>()
            .ForMember(dest => dest.IngredientTitle, opt => opt.MapFrom(src => src.Ingredient!.Name)); // (!)

        CreateMap<Recipe, RecipeVm>()
            .ForCtorParam(nameof(RecipeVm.Ingredients),
            opt => opt.MapFrom(src => src.Ingredients))
            .ForCtorParam(nameof(RecipeVm.AverageRating),
            opt => opt.MapFrom(src => src.Raitings.Count > 0 ? src.Raitings.Average() : (double?)null));

        CreateMap<IngredientInRecipe, IngredientInRecipeCreateVm>();

        CreateMap<Recipe, RecipesListVm>()
            .ForCtorParam(nameof(RecipesListVm.AverageRating),
            opt => opt.MapFrom(src => src.Raitings.Count > 0 ? src.Raitings.Average() : (double?)null));

        CreateMap<IEnumerable<Recipe>, ListOfRecipes>()
            .ForCtorParam(nameof(ListOfRecipes.Recipes), opt => opt.MapFrom(src => src.ToList()));

        CreateMap<IngredientInRecipeCreateVm, IngredientInRecipe>()
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.Recipe, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredient, opt => opt.Ignore());

        CreateMap<CreateRecipeDto, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Raitings, opt => opt.Ignore())
            .ForMember(dest => dest.CookTime, opt => opt.Ignore()) // будет потом заполнен в репозитории т.к. нужен TimeConverter
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));

        // В примере из лекции такого маппинга нет и я хуй знает нужен ли он
        CreateMap<UpdateRecipeDto, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Raitings, opt => opt.Ignore())
            .ForMember(dest => dest.CookTime, opt => opt.Ignore()) // будет потом заполнен в репозитории
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));
    }

}
*/
