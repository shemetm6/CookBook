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
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IApplicationDbContext applicationDbContext, 
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<AuthService> logger
        )
    {
        _applicationDbContext = applicationDbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<LogInResponse?> LogInAsync(LogInDto dto)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Login == dto.Login);

        if (user is null)
        {
            _logger.LogWarning("Failed login. User with login {Login} not found.", dto.Login);
            return null;
        }

        if (!PasswordHasher.VerifyPassword(user.Password, dto.Password))
        {
            _logger.LogWarning("Failed login. Wrong password for user {Id}.", user.Id);
            return null;
        }

        var (jwt, refresh) = await UpdateTokenAsync(user);

        await _applicationDbContext.SaveChangesAsync();

        _logger.LogInformation("User {Id} successfully logged in", user.Id);

        return CreateResponse(jwt, refresh);
    }

    public async Task<bool> LogOutAsync(int userId)
    {
        var user = await _applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            _logger.LogWarning("Failed logout. User with login {UserId} not found.", userId);
            return false;
        }

        var token = await _applicationDbContext.JwtTokens
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (token is null)
        {
            _logger.LogWarning("Failed logout. Couldn't get token for user {UserId}.", userId);
            return false;
        }

        _applicationDbContext.JwtTokens.Remove(token);
        await _applicationDbContext.SaveChangesAsync();

        _logger.LogInformation("User {UserId} successfully logged out.", userId);

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

        _logger.LogInformation("User {Id} successfully signed up.", user.Id);

        return CreateResponse(jwt, refresh);
    }

    public async Task<bool> VerifyTokenAsync(int userId, string token)
    {
        var jwtToken = await _applicationDbContext.JwtTokens
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (jwtToken is null)
        {
            _logger.LogWarning("Failed verifying. Couldn't get token for user {UserId}.", userId);
            return false;
        }

        var isValid = jwtToken.Token == token && jwtToken.ExpiresAt > DateTime.UtcNow;

        if(!isValid)
            _logger.LogWarning("Failed token verification for user {UserId}.", userId);
        else
            _logger.LogInformation("Token successfully verified for user {UserId}.", userId);

        return isValid;
    }

    public async Task<LogInResponse?> RefreshAsync(string refreshToken)
    {
        var existingRefreshToken = await _applicationDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

        if (existingRefreshToken is null)
        {
            _logger.LogWarning("Failed refreshing. Couldn't get refresh token.");
            return null;
        }

        var (jwt, refresh) = await UpdateTokenAsync(existingRefreshToken.User);

        await _applicationDbContext.SaveChangesAsync();

        _logger.LogInformation("Token successfully refreshed for user {UserId}.", existingRefreshToken.UserId);

        return CreateResponse(jwt, refresh);
    }

    public async Task RevokeAsync(string refreshToken)
    {
        var existingRefreshToken = await _applicationDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

        if (existingRefreshToken is null)
        {
            _logger.LogWarning("Failed refreshing. Couldn't get refresh token.");
            return;
        }

        _applicationDbContext.RefreshTokens.Remove(existingRefreshToken);

        await _applicationDbContext.SaveChangesAsync();
        
        _logger.LogInformation("Token successfully revoked for user {UserId}.", existingRefreshToken.UserId);
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

        _logger.LogInformation("Token successfully updated for user {Id}.", user.Id);

        return (token, refreshToken);
    }

    private static LogInResponse CreateResponse(JwtToken jwt, RefreshToken refresh)
        => new(jwt.UserId, jwt.Token, refresh.Token);
}
