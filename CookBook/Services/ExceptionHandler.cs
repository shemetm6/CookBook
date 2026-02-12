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
        httpContext.Response.StatusCode = exception switch
        {
            RecipeNotFoundException => (int)HttpStatusCode.NotFound,
            RecipeIdDuplicateException => (int)HttpStatusCode.Conflict,
            IngredientNotFoundException => (int)HttpStatusCode.NotFound,
            UserNotFoundException => (int)HttpStatusCode.NotFound,
            ArgumentOutOfRangeException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError,
        };

        // Вынес логгирование сюда т.к. это единственное исключение,
        // которое не логгируется в том сервисе, в котором выбрасывается
        // А не логгируется оно там потому что я не понял как его в этот новомодный switch засунуть лол
        // Это вообще нормально выглядит или странно?
        if (exception is ArgumentOutOfRangeException)
            logger.LogError(exception, "Invalid argument.");

        // С вот этим логгированием у меня вопросов нет.
        if (httpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            logger.LogError(exception, "An unexpected exception occurred.");

        await httpContext.Response.WriteAsync(exception.Message);

        return true;
    }
}
