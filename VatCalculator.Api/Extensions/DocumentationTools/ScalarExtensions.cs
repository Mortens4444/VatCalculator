using Scalar.AspNetCore;
using System.Diagnostics.CodeAnalysis;
using VatCalculator.Shared;

namespace VatCalculator.Api.Extensions.DocumentationTools;

public static class ScalarExtensions
{
    [ExcludeFromCodeCoverage]
    public static IApplicationBuilder UseScalarDocumentation(this WebApplication app, string version)
    {
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle($"{Constants.ProgramName} API {version}")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithOpenApiRoutePattern($"/openapi/{version}.json")
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
        return app;
    }
}
