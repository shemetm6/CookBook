using CookBook.Abstractions;
using CookBook.Models;
using CookBook.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CookBook.Services;

public class JwtTokenGenerator(IOptions<JwtOptions> options) : IJwtTokenGenerator
{
    private readonly JwtOptions _options = options.Value;

    // В лекции этот метод почему-то возвращает string (стр.9)
    public JwtToken Generate(User user)
    {
        SigningCredentials credentials = new(
            new SymmetricSecurityKey(Convert.FromBase64String(_options.Secret)),
            SecurityAlgorithms.HmacSha256
            );

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.Login),
            // А нам точно нужен FamilyName? У нас ведь только логин
            new Claim(JwtRegisteredClaimNames.FamilyName, user.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var now = DateTime.UtcNow;
        var expiration = now.AddMinutes(5);

        // Я в ахуе, что так можно заполнять свойства при создании объекта
        JwtSecurityToken securityToken = new(
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: expiration,
            claims: claims,
            signingCredentials: credentials
            );

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return new JwtToken
        {
            UserId = user.Id,
            Token = token,
            CreatedAt = now,
            ExpiresAt = expiration,
        };
    }

    public RefreshToken GetRefreshToken(int userId) => new()
    {
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
        UserId = userId,
        ExpiresAt = DateTime.UtcNow.AddDays(7),
    };
}
