using AutoMapper;
using CookBook.Models;
using CookBook.Contracts;

namespace CookBook.Configurations.Mapping;

public class IngredientMappingProfile : Profile
{
    public IngredientMappingProfile()
    {
        CreateMap<IngredientInRecipe, RecipeInIngredientVm>()
            .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.RecipeId))
            .ForMember(dest => dest.RecipeTitle, opt => opt.MapFrom(src => src.Recipe!.Title));

        CreateMap<Ingredient, IngredientVm>()
            .ForCtorParam(nameof(IngredientVm.Recipes), opt => opt.MapFrom(src => src.Recipes));

        CreateMap<Ingredient, IngredientInListVm>();

        CreateMap<IEnumerable<Ingredient>, ListOfIngredients>()
            .ForCtorParam(nameof(ListOfIngredients.Ingredients),
            src => src.MapFrom(src => src.ToList()));

        CreateMap<CreateIngredientDto, Ingredient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Recipes, opt => opt.Ignore());
    }
}
