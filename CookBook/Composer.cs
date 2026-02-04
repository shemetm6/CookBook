using CookBook.Services;
using CookBook.Extensions;
using CookBook.Database;
using CookBook.Abstractions;
using CookBook.Configurations.Database;
using CookBook.Configurations;
using CookBook.Politics;
using System.Text.Json.Serialization;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace CookBook;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration
        )
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetRequiredSection(nameof(JwtOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration
                .GetRequiredSection(nameof(JwtOptions))
                .Get<JwtOptions>()!;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtOptions.Secret)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var authService = context.HttpContext.RequestServices
                        .GetRequiredService<IAuthService>();

                        var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (userId is null
                        || context.SecurityToken.ValidTo < DateTime.UtcNow
                        || !authService.VerifyToken(int.Parse(userId),
                            context.SecurityToken.UnsafeToString()))
                        {
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddScoped<IAuthorizationHandler, RecipeOwnerRequirementHandler>();
        services.AddHttpContextAccessor();
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder =
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

            defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

            options.AddPolicy("RecipeOwner", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new RecipeOwnerRequirement());
            });
        });

        services.AddValidatorsFromAssembly(typeof(Composer).Assembly);
        services.AddFluentValidationAutoValidation();

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

        services.AddSwaggerGen()
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
            .AddAuthorization()
            .AddAuthentication();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTimeConverter();
        services.AddIngredientService();
        services.AddRecipeService();
        services.AddUserService();
        services.AddAuthService();
        services.AddJwtTokenGenerator();

        return services;
    }
}
