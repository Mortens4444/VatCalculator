using AspNetCore.Swagger.Themes;
using Microsoft.OpenApi;
using System.Diagnostics.CodeAnalysis;
using VatCalculator.Shared;

namespace VatCalculator.Api.Extensions.DocumentationTools;

public static class SwaggerExtensions
{
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, string version)
    {
        services.AddSwaggerGen(c =>
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = $"{Constants.ProgramName} API {version}",
                Version = version
            };
            c.SwaggerDoc(version, openApiInfo);
            var filePath = Path.Combine(AppContext.BaseDirectory, $"{Constants.ProgramName}.Api.xml");
            if (File.Exists(filePath))
            {
                c.IncludeXmlComments(filePath);
            }
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this WebApplication app, string version)
    {
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "openapi/{documentName}.json";
        });

        app.UseSwaggerUI(Theme.Dark, c =>
        {
            c.SwaggerEndpoint($"/openapi/{version}.json", $"{Constants.ProgramName} API {version}");
            c.RoutePrefix = "swagger";
            c.EnableAllAdvancedOptions();
        });
        return app;
    }
}
