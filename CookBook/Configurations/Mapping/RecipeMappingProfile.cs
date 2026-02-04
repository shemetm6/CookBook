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
            opt => opt.MapFrom(src => src.Ingredient!.Name));

        CreateMap<Recipe, RecipeVm>()
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

        CreateMap<IngredientInRecipeCreateVm, IngredientInRecipe>();

        CreateMap<CreateRecipeDto, Recipe>()
            .ForMember(dest => dest.CookTime, opt => opt.Ignore());

        CreateMap<UpdateRecipeDto, Recipe>()
            .ForMember(dest => dest.CookTime, opt => opt.Ignore());
    }
}
