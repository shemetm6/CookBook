using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IAuthService
{
    Task<LogInResponse> SignUpAsync(SignUpDto dto);
    Task<LogInResponse?> LogInAsync(LogInDto dto);
    Task<bool> LogOutAsync(int userId);
    Task<bool> VerifyTokenAsync(int userId, string token);
    Task<LogInResponse?> RefreshAsync(string refreshToken);
    Task RevokeAsync(string refreshToken);
}
