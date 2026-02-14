using AutoMapper;
using CookBook.Configurations.Mapping;

namespace CookBook.Tests;

public class AutoMapperProfilesTests
{
    [Fact]
    public void ProfileConfuguration_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<IngredientMappingProfile>();
            cfg.AddProfile<RecipeMappingProfile>();
            cfg.AddProfile<UserMappingProfile>();
        });

        config.AssertConfigurationIsValid();
    }
}
