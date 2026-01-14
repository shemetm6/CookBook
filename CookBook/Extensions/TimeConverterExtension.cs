using CookBook.Abstractions;
using CookBook.Services;

namespace CookBook.Extensions;

public static class TimeConverterExtension
{
    public static IServiceCollection AddTimeConverter(this IServiceCollection services)
        => services.AddSingleton<ITimeConverter, TimeConverter>();
}
