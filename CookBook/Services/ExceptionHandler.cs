using CookBook.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace CookBook.Services;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
        )
    {
        switch (exception)
        {
            case RecipeNotFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case RecipeIdDuplicateException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;
            case IngredientNotFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case UserNotFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case ArgumentOutOfRangeException:
                logger.LogError(exception, "Invalid argument.");
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            default:
                logger.LogError(exception, "An unexpected exception occurred.");
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        await httpContext.Response.WriteAsync(exception.Message);

        return true;
    }
}
