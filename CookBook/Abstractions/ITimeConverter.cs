using CookBook.Enums;

namespace CookBook.Abstractions;

public interface ITimeConverter
{
    TimeSpan Convert(double time, TimeUnit unit);
}
