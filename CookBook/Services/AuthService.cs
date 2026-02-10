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

    public async Task<LogInResponse?> LogInAsync(LogInDto dto)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Login == dto.Login);

        if (user is null)
            return null;

        if (!PasswordHasher.VerifyPassword(user.Password, dto.Password))
            return null;
        
        var (jwt, refresh) = await UpdateTokenAsync(user);

        await _applicationDbContext.SaveChangesAsync();

        return CreateResponse(jwt, refresh);
    }

    public async Task<bool> LogOutAsync(int userId)
    {
        var user = await _applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return false;

        var token = await _applicationDbContext.JwtTokens
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (token is null)
            return false;

        _applicationDbContext.JwtTokens.Remove(token);
        await _applicationDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<LogInResponse> SignUpAsync(SignUpDto dto)
    {
        var user = new User
        {
            Login = dto.Login,
            Password = PasswordHasher.HashPassword(dto.Password),
        };

        await _applicationDbContext.Users.AddAsync(user);
        await _applicationDbContext.SaveChangesAsync();

        var (jwt, refresh) = await UpdateTokenAsync(user);

        await _applicationDbContext.SaveChangesAsync();

        return CreateResponse(jwt, refresh);
    }

    public async Task<bool> VerifyTokenAsync(int userId, string token)
    {
        var jwtToken = await _applicationDbContext.JwtTokens
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (jwtToken is null)
            return false;

        return jwtToken.Token == token && jwtToken.ExpiresAt > DateTime.UtcNow;
    }

    public async Task<LogInResponse?> RefreshAsync(string refreshToken)
    {
        var existingRefreshToken = await _applicationDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

        if (existingRefreshToken is null)
            return null;

        var (jwt, refresh) = await UpdateTokenAsync(existingRefreshToken.User);

        await _applicationDbContext.SaveChangesAsync();

        return CreateResponse(jwt, refresh);
    }

    public async Task RevokeAsync(string refreshToken)
    {
        var existingRefreshToken = await _applicationDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

        if (existingRefreshToken is null)
            return;

        _applicationDbContext.RefreshTokens.Remove(existingRefreshToken);
        await _applicationDbContext.SaveChangesAsync();
    }

    private async Task<(JwtToken Jwt, RefreshToken Refresh)> UpdateTokenAsync(User user)
    {
        var token = _jwtTokenGenerator.Generate(user);

        var oldToken = await _applicationDbContext.JwtTokens
            .FirstOrDefaultAsync(t => t.UserId == user.Id);
        
        if (oldToken is not null)
            _applicationDbContext.JwtTokens.Remove(oldToken);

        await _applicationDbContext.JwtTokens.AddAsync(token);

        var refreshToken = _jwtTokenGenerator.GetRefreshToken(user.Id);

        await _applicationDbContext.RefreshTokens.AddAsync(refreshToken);

        return (token, refreshToken);
    }

    // (todo) По желанию сделать Mapping Profile 
    private static LogInResponse CreateResponse(JwtToken jwt, RefreshToken refresh)
        => new(jwt.UserId, jwt.Token, refresh.Token);
}
