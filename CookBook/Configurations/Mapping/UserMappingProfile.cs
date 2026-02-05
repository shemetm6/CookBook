using CookBook.Models;
using CookBook.Contracts;
using AutoMapper;

namespace CookBook.Configurations.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Recipe, RecipeInUserVm>()
            .ForCtorParam(nameof(RecipeInUserVm.AverageRating),
            opt => opt.MapFrom(src => src.Ratings.Count > 0 ? src.Ratings.Select(rating => rating.Value).Average() : (double?)null));

        CreateMap<Rating, RatingInUserVm>()
            .ForCtorParam(nameof(RatingInUserVm.RecipeTitle),
            opt => opt.MapFrom(src => src.Recipe.Title));

        CreateMap<User, UserVm>();

        CreateMap<User, UserInListVm>();

        CreateMap<IEnumerable<User>, ListOfUsers>()
            .ForCtorParam(nameof(ListOfUsers.Users),
            opt => opt.MapFrom(src => src.ToList()));

        CreateMap<UpdateUserDto, User>();
    }
}
