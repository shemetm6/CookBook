using CookBook.Models;
using CookBook.Contracts;
using AutoMapper;

namespace CookBook.Configurations.Mapping;

public class IngredientMappingProfile : Profile
{
    public IngredientMappingProfile()
    {
        CreateMap<IngredientInRecipe, RecipeInIngredientVm>()
            .ForMember(dest => dest.RecipeTitle, opt => opt.MapFrom(src => src.Recipe.Title));

        CreateMap<Ingredient, IngredientVm>();

        CreateMap<Ingredient, IngredientInListVm>();

        CreateMap<IEnumerable<Ingredient>, ListOfIngredients>()
            .ForCtorParam(nameof(ListOfIngredients.Ingredients),
            src => src.MapFrom(src => src.ToList()));

        CreateMap<CreateIngredientDto, Ingredient>();
    }
}
