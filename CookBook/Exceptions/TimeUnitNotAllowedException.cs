using CookBook.Enums;

namespace CookBook.Exceptions;

// Есть ощущение, что я это исключение не смогу увидеть ни в каком случае
// Может оно и не нужно?
public class TimeUnitNotAllowedException : Exception
{
    public TimeUnitNotAllowedException(TimeUnit invalidTimeUnit) : base(BuildMessage(invalidTimeUnit)) { }

    private static string BuildMessage(TimeUnit invalidTimeUnit)
    {
        var allowedTimeUnits = string.Join(", ", Enum.GetNames<TimeUnit>());

        return $"Following time unit is not allowed: {invalidTimeUnit}.\n" +
               $"List of allowed time units: {allowedTimeUnits}.";
    }
}
