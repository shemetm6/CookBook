using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CookBook.Politics;

public class RecipeOwnerRequirementHandler(IHttpContextAccessor accessor)
    : AuthorizationHandler<RecipeOwnerRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        RecipeOwnerRequirement requirement
        )
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if(userIdClaim is null)
        {
            context.Fail();

            return Task.CompletedTask;
        }

        var accessorResult = accessor
            .HttpContext!
            .Request
            .Query
            .TryGetValue("userId", out var userIdQuery);

        if(!accessorResult || !userIdQuery.Any())
        {
            context.Fail();

            return Task.CompletedTask;
        }

        if (userIdClaim.Value != userIdQuery.First())
        {
            context.Fail();

            return Task.CompletedTask;
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
