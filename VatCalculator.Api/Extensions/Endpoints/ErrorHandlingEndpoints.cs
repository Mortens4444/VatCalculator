using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using VatCalculator.Shared;

namespace VatCalculator.Api.Extensions.Endpoints;

/// <summary>
/// Provides centralized error handling endpoints.
/// </summary>
public static class ErrorHandlingEndpoints
{
    /// <summary>
    /// Maps global error handling and test-only error simulation endpoints.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public static void MapErrorHandlingEndpoints(this IEndpointRouteBuilder app)
    {
        app.Map("/error", HandleError)
        .WithName("GlobalErrorHandler")
        .WithSummary("Global error handler")
        .WithDescription(
            "Centralized error handling endpoint used by the ASP.NET Core exception middleware. This endpoint is not intended to be called directly."
        )
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        var env = app.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        if (env.IsEnvironment("Test"))
        {
            app.MapGet(Constants.Throw, (_) => throw new InvalidOperationException("Boom!"))
            .WithName("ThrowTestException")
            .WithSummary("Throws a test exception")
            .WithDescription("⚠️ FOR TESTING PURPOSES ONLY. This endpoint intentionally throws an unhandled exception to validate global error handling behavior. It MUST NOT be enabled in production environments.")
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }

    [ExcludeFromCodeCoverage]
    private static IResult HandleError(HttpContext ctx)
    {
        var exception = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Results.Problem(
            title: "Unexpected error",
            detail: exception?.Message,
            statusCode: StatusCodes.Status500InternalServerError);
    }
}
