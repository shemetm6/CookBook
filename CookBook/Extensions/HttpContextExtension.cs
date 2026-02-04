using System.Security.Claims;

namespace CookBook.Extensions;

public static class HttpContextExtension
{
    public static int? ExtractUserIdFromClaims(this HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(claim 
            => claim.Type == ClaimTypes.NameIdentifier);

        if (claim is null)
            return null;

        return int.Parse(claim.Value);
    }
}
