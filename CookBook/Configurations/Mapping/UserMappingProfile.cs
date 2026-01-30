using AutoMapper;
using CookBook.Models;
using CookBook.Contracts;

namespace CookBook.Configurations.Mapping;

// Я чет запутался с настройкой маппинга. Вот здесь ни указано вообще никаких дополнительных условий т.к. всё маппится 1 к 1
// Единственное, что осталось это преобразование IEnumerable к List. И теперь я не уверен, а нужно ли оно
// т.к. ICollection<Recipe> нормально смаппился в List<RecipeInUserVm> без дополнительных указаний
// И всё работает!
// Есть подозрение, что из оставшихся двух профилей можно выкинуть вообще всё кроме тех настроек,
// где есть Ignore и маппинг св-в, у которых названия отличаются у модели и контракта
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Recipe, RecipeInUserVm>();

        CreateMap<User, UserVm>();

        CreateMap<User, UserInListVm>();

        CreateMap<IEnumerable<User>, ListOfUsers>()
            .ForCtorParam(nameof(ListOfUsers.Users), opt => opt.MapFrom(src => src.ToList()));

        CreateMap<CreateUserDto, User>();

        CreateMap<UpdateUserDto, User>();
    }
}
