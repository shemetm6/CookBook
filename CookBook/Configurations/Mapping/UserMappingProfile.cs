using CookBook.Models;
using CookBook.Contracts;
using AutoMapper;

namespace CookBook.Configurations.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Recipe, RecipeInUserVm>();

        CreateMap<User, UserVm>();

        CreateMap<User, UserInListVm>();

        CreateMap<IEnumerable<User>, ListOfUsers>()
            .ForCtorParam(nameof(ListOfUsers.Users), opt => opt.MapFrom(src => src.ToList()));

        CreateMap<UpdateUserDto, User>();
    }
}
