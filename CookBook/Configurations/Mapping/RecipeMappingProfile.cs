using CookBook.Models;
using CookBook.Contracts;
using AutoMapper;

namespace CookBook.Configurations.Mapping;

public class RecipeMappingProfile : Profile
{
    public RecipeMappingProfile()
    {
        CreateMap<IngredientInRecipe, IngredientInRecipeVm>()
            .ForCtorParam(nameof(IngredientInRecipeVm.IngredientName),
            opt => opt.MapFrom(src => src.Ingredient.Name));

        CreateMap<Recipe, RecipeVm>()
            .ForCtorParam(nameof(RecipeVm.UserLogin),
            opt => opt.MapFrom(src => src.User.Login))
            .ForCtorParam(nameof(RecipeVm.AverageRating),
            opt => opt.MapFrom(src => src.Ratings.Count > 0 ? src.Ratings.Select(r => r.Value).Average() : (double?)null));

        CreateMap<IngredientInRecipe, IngredientInRecipeCreateVm>();

        CreateMap<Recipe, RecipeInListVm>()
            .ForCtorParam(nameof(RecipeInListVm.Author),
            opt => opt.MapFrom(src => src.User.Login))
            .ForCtorParam(nameof(RecipeInListVm.AverageRating),
            opt => opt.MapFrom(src => src.Ratings.Count > 0 ? src.Ratings.Select(r => r.Value).Average() : (double?)null));

        CreateMap<IEnumerable<Recipe>, ListOfRecipes>()
            .ForCtorParam(nameof(ListOfRecipes.Recipes), opt => opt.MapFrom(src => src.ToList()));

        CreateMap<IngredientInRecipeCreateVm, IngredientInRecipe>()
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.Recipe, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredient, opt => opt.Ignore());

        CreateMap<CreateRecipeDto, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CookTime, opt => opt.Ignore())
            .ForMember(dest => dest.Ratings, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<UpdateRecipeDto, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CookTime, opt => opt.Ignore())
            .ForMember(dest => dest.Ratings, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<RateRecipeDto, Rating>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());
    }
}
