using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace VatCalculator.Api.Services;

[ExcludeFromCodeCoverage]
public class GlobalExceptionHandler(IWebHostEnvironment env, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.Clear();
        var errorMessage = "An unexpected error occurred.";
        var statusCode = StatusCodes.Status500InternalServerError;

        if (exception is BadHttpRequestException || exception is JsonException)
        {
            statusCode = StatusCodes.Status400BadRequest;

            var match = Regex.Match(exception.Message, @"Path: \$\.(?<field>[a-zA-Z0-9_]+)");
            var fieldName = match.Success ? match.Groups["field"].Value : "unknown";

            errorMessage = fieldName != "unknown"
                ? $"Invalid value or format for field: '{fieldName}'."
                : "The request body contains invalid JSON or incorrect data types.";
        }

        httpContext.Response.StatusCode = statusCode;

        var response = new Dictionary<string, object>
        {
            { "error", errorMessage }
        };

        if (env.IsDevelopment())
        {
            response.Add("message", exception.Message);
            response.Add("stackTrace", exception.StackTrace ?? String.Empty);
        }

        logger.LogError(exception, "Error handled: {Message}", exception.Message);

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}