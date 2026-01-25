using AutoMapper;
using CookBook.Models;
using CookBook.Contracts;

namespace CookBook.Configurations.Mapping;

public class RecipeMappingProfile : Profile
{
    public RecipeMappingProfile()
    {
        // Мне казалось, что тут автоматически смаппится/проигнорируется всё, кроме IngredientName т.к. нет такого св-ва в IngredientInRecipe
        // Но подробно посмотреть рецепт через get запрос по id не получалось, выдавал ошибку маппинга
        CreateMap<IngredientInRecipe, IngredientInRecipeVm>()
            .ForCtorParam(nameof(IngredientInRecipeVm.IngredientId),
            opt => opt.MapFrom(src => src.IngredientId))
            .ForCtorParam(nameof(IngredientInRecipeVm.IngredientName),
            opt => opt.MapFrom(src => src.Ingredient!.Name))
            .ForCtorParam(nameof(IngredientInRecipeVm.Quantity),
            opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam(nameof(IngredientInRecipeVm.Units),
            opt => opt.MapFrom(src => src.Units));

        CreateMap<Recipe, RecipeVm>()
            .ForCtorParam(nameof(RecipeVm.Ingredients),
            opt => opt.MapFrom(src => src.Ingredients))
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
            // Будет потом заполнен в репозитории т.к. нужен TimeConverter
            // (!!!) А репозиторий то теперь не будет использоваться т.к. у нас ApplicationDbContext есть.
            .ForMember(dest => dest.CookTime, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));

        CreateMap<UpdateRecipeDto, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ratings, opt => opt.Ignore())
            // Будет потом заполнен в репозитории т.к. нужен TimeConverter
            // Та же проблема
            .ForMember(dest => dest.CookTime, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));
    }

}