namespace CookBook.Contracts;

public record LogInResponse(int UserId, string Token, string RefreshToken);
