using CookBook.Services;
using System.Text.Json.Serialization;
using CookBook.Extensions;
using CookBook.Database;
using CookBook.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CookBook;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Composer).Assembly);
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=CookBook"
                );
        });
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTimeConverter();
        services.AddIngredientRepository();
        services.AddIngredientService();
        services.AddIngredientInRecipeService();
        services.AddIngredientInRecipeRepository();
        services.AddRecipeService();
        services.AddRecipeRepository();

        return services;
    }
}
