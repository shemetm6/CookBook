using AutoMapper;
using CookBook.Models;
using CookBook.Contracts;

namespace CookBook.Configurations.Mapping;

public class RecipeMappingProfile : Profile
{
    public RecipeMappingProfile()
    {
        // Меня смущает объем этого блока. Но! Чатгпт сказал, что если ты один раз вызываешь ForCtorParam,
        // то AutoMapper считает, что ты взял управление на себя и сам уже ничего не сопоставляет.
        // И у меня действительно не получалось сделать get запрос по id рецепта, пока я это всё не прописал
        CreateMap<IngredientInRecipe, IngredientInRecipeVm>()
            .ForCtorParam(nameof(IngredientInRecipeVm.IngredientId),
            opt => opt.MapFrom(src => src.IngredientId))
            .ForCtorParam(nameof(IngredientInRecipeVm.IngredientName),
            opt => opt.MapFrom(src => src.Ingredient!.Name))
            .ForCtorParam(nameof(IngredientInRecipeVm.Quantity),
            opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam(nameof(IngredientInRecipeVm.Units),
            opt => opt.MapFrom(src => src.Units));

        // Но тогда какого хуя тут всё маппится нормально. Здесь указаны далеко не все свойства
        // (для себя) Попробуй избавиться от лишнего после коммита
        CreateMap<Recipe, RecipeVm>()
            .ForCtorParam(nameof(RecipeVm.Ingredients),
            opt => opt.MapFrom(src => src.Ingredients))
            .ForCtorParam(nameof(RecipeVm.UserLogin),
            opt => opt.MapFrom(src => src.User.Login))
            .ForCtorParam(nameof(RecipeVm.AverageRating),
            opt => opt.MapFrom(src => src.Ratings.Count > 0 ? src.Ratings.Average() : (double?)null));

        CreateMap<IngredientInRecipe, IngredientInRecipeCreateVm>();

        CreateMap<Recipe, RecipeInListVm>()
            .ForCtorParam(nameof(RecipeInListVm.AverageRating),
            opt => opt.MapFrom(src => src.Ratings.Count > 0 ? src.Ratings.Average() : (double?)null));

        CreateMap<IEnumerable<Recipe>, ListOfRecipes>()
            .ForCtorParam(nameof(ListOfRecipes.Recipes), opt => opt.MapFrom(src => src.ToList()));

        CreateMap<IngredientInRecipeCreateVm, IngredientInRecipe>()
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.Recipe, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredient, opt => opt.Ignore());

        CreateMap<CreateRecipeDto, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ratings, opt => opt.Ignore())
            .ForMember(dest => dest.CookTime, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));

        CreateMap<UpdateRecipeDto, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ratings, opt => opt.Ignore())
            .ForMember(dest => dest.CookTime, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));
    }
}