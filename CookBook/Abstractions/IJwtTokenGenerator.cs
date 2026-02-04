using CookBook.Models;

namespace CookBook.Abstractions;

public interface IJwtTokenGenerator
{
    JwtToken Generate(User user);
    RefreshToken GetRefreshToken(int userId);
}
