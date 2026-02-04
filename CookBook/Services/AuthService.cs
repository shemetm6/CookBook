using CookBook.Contracts;
using CookBook.Abstractions;
using CookBook.Models;
using CookBook.Utils;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IApplicationDbContext applicationDbContext, 
        IJwtTokenGenerator jwtTokenGenerator
        )
    {
        _applicationDbContext = applicationDbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public LogInResponse? LogIn(LogInDto dto)
    {
        var user = _applicationDbContext.Users
            .FirstOrDefault(u => u.Login == dto.Login);

        if (user is null)
            return null;

        if (!PasswordHasher.VerifyPassword(user.Password, dto.Password))
            return null;
        
        var (jwt, refresh) = UpdateToken(user);

        _applicationDbContext.SaveChanges();

        return CreateResponse(jwt, refresh);
    }

    public bool LogOut(int userId)
    {
        var user = _applicationDbContext.Users
            .FirstOrDefault(u => u.Id == userId);

        if (user is null)
            return false;

        var token = _applicationDbContext.JwtTokens
            .FirstOrDefault(t => t.UserId == userId);

        if (token is null)
            return false;

        _applicationDbContext.JwtTokens.Remove(token);
        _applicationDbContext.SaveChanges();

        return true;
    }

    public LogInResponse SignUp(SignUpDto dto)
    {
        var user = new User
        {
            Login = dto.Login,
            Password = PasswordHasher.HashPassword(dto.Password),
        };

        _applicationDbContext.Users.Add(user);
        _applicationDbContext.SaveChanges();

        var (jwt, refresh) = UpdateToken(user);

        _applicationDbContext.SaveChanges();

        return CreateResponse(jwt, refresh);
    }

    public bool VerifyToken(int userId, string token)
    {
        var jwtToken = _applicationDbContext.JwtTokens
            .FirstOrDefault(t => t.UserId == userId);

        if (jwtToken is null)
            return false;

        return jwtToken.Token == token && jwtToken.ExpiresAt > DateTime.UtcNow;
    }

    public LogInResponse? Refresh(string refreshToken)
    {
        var existingRefreshToken = _applicationDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefault(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

        if (existingRefreshToken is null)
            return null;

        var (jwt, refresh) = UpdateToken(existingRefreshToken.User);

        _applicationDbContext.SaveChanges();

        return CreateResponse(jwt, refresh);
    }

    public void Revoke(string refreshToken)
    {
        var existingRefreshToken = _applicationDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefault(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

        if (existingRefreshToken is null)
            return;

        _applicationDbContext.RefreshTokens.Remove(existingRefreshToken);
        _applicationDbContext.SaveChanges();
    }

    private (JwtToken Jwt, RefreshToken Refresh) UpdateToken(User user)
    {
        var token = _jwtTokenGenerator.Generate(user);

        var oldToken = _applicationDbContext.JwtTokens
            .FirstOrDefault(t => t.UserId == user.Id);
        
        if (oldToken is not null)
            _applicationDbContext.JwtTokens.Remove(oldToken);

        _applicationDbContext.JwtTokens.Add(token);

        var refreshToken = _jwtTokenGenerator.GetRefreshToken(user.Id);

        _applicationDbContext.RefreshTokens.Add(refreshToken);

        return (token, refreshToken);
    }

    // Есть ощущение, что стоит использовать автомаппер, раз он есть
    private static LogInResponse CreateResponse(JwtToken jwt, RefreshToken refresh)
        => new(jwt.UserId, jwt.Token, refresh.Token);
}
