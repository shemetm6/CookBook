using CookBook.Enums;

namespace CookBook.Abstractions;

public interface ITimeConverter
{
    public TimeSpan Convert(double time, TimeUnit unit);
}
