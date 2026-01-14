using CookBook.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace CookBook.Services;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            RecipeNotFoundException => (int)HttpStatusCode.NotFound,
            RecipeIdDuplicateException => (int)HttpStatusCode.Conflict,
            IngredientNotAllowedException => (int)HttpStatusCode.BadRequest,
            TimeUnitNotAllowedException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError,
        };

        await httpContext.Response.WriteAsync(exception.Message);

        return true;
    }
}
