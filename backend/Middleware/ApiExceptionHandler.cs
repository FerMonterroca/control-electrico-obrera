using ControlElectricoObrera.Api.DTOs;
using Microsoft.AspNetCore.Diagnostics;

namespace ControlElectricoObrera.Api.Middleware;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            "Unhandled API exception of type {ExceptionType}. Trace identifier: {TraceIdentifier}",
            exception.GetType().Name,
            httpContext.TraceIdentifier);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            ApiErrorResponse.Create(
                "internal_error",
                "An unexpected error occurred."),
            cancellationToken);

        return true;
    }
}
