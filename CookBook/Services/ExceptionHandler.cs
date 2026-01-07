using CookBook.Exceptions;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace CookBook.Services;

public class ExceptionHandler : IExceptionHandler // Что тут вообще происходит. Разберись
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is RecipeNotFoundException recipeNotFoundException)
        {
            httpContext.Response.ContentType = MediaTypeNames.Text.Plain;

            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

            await httpContext.Response.WriteAsync(recipeNotFoundException.Message);

            return true;
        }
        else if (exception is RecipeIdDuplicateException recipeIdDuplicateException)
        {
            httpContext.Response.ContentType = MediaTypeNames.Text.Plain;

            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;

            await httpContext.Response.WriteAsync(recipeIdDuplicateException.Message);

            return true;
        }
        else if (exception is IngredientNotAllowedException ingredientNotAllowedException)
        {
            httpContext.Response.ContentType = MediaTypeNames.Text.Plain;

            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await httpContext.Response.WriteAsync(ingredientNotAllowedException.Message);

            return true;
        }

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await httpContext.Response.WriteAsync(string.Empty);

        return false;
    }
}
