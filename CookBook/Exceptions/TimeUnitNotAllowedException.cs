using CookBook.Enums;

namespace CookBook.Exceptions;

public class TimeUnitNotAllowedException : Exception
{
    public TimeUnitNotAllowedException(TimeUnit invalidTimeUnit) : base(BuildMessage(invalidTimeUnit))
    { }

    private static string BuildMessage(TimeUnit invalidTimeUnit)
    {
        var allowedTimeUnits = string.Join(", ", Enum.GetNames<TimeUnit>());

        return $"Following time unit is not allowed: {invalidTimeUnit}.\n" +
               $"List of allowed time units: {allowedTimeUnits}.";
    }
}
