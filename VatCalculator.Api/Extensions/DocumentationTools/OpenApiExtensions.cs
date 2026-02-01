using System.Diagnostics.CodeAnalysis;

namespace VatCalculator.Api.Extensions.DocumentationTools;

public static class OpenApiExtensions
{
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApi(options =>
        {
            options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_1;
        });
        return services;
    }
}
