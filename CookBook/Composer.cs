using CookBook.Services;
using System.Text.Json.Serialization;
using CookBook.Extensions;
using CookBook.Database;
using CookBook.Abstractions;
using Microsoft.EntityFrameworkCore;
using CookBook.Configurations.Database;

namespace CookBook;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration
        )
    {
        services.AddAutoMapper(typeof(Composer).Assembly);

        services.AddOptions<ApplicationDbContextSettings>()
            .Bind(configuration.GetRequiredSection(nameof(ApplicationDbContextSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.Configure<ApplicationDbContextSettings>(
            configuration.GetRequiredSection(nameof(ApplicationDbContextSettings)));

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();
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
        services.AddIngredientService();
        services.AddRecipeService();

        return services;
    }
}
