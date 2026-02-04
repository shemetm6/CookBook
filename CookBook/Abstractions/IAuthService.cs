using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IAuthService
{
    LogInResponse SignUp(SignUpDto dto);
    LogInResponse? LogIn(LogInDto dto);
    bool LogOut(int userId);
    bool VerifyToken(int userId, string token);
    LogInResponse? Refresh(string refreshToken);
    void Revoke(string refreshToken);
}
