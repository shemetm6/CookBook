using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Abstractions;

namespace CookBook.Services;

public class TimeConverter : ITimeConverter
{
    public TimeSpan Convert(double time, TimeUnit unit)
    {
        return unit switch
        {
            TimeUnit.Seconds => TimeSpan.FromSeconds(time),
            TimeUnit.Minutes => TimeSpan.FromMinutes(time),
            TimeUnit.Hours => TimeSpan.FromHours(time),
            TimeUnit.Days => TimeSpan.FromDays(time),
            _ => throw new TimeUnitNotAllowedException(unit),
        };
    }
}
