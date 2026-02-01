using Scalar.AspNetCore;
using System.Diagnostics.CodeAnalysis;
using VatCalculator.Api;
using VatCalculator.Api.Extensions;
using VatCalculator.Api.Extensions.DocumentationTools;
using VatCalculator.Api.Extensions.Endpoints;
using VatCalculator.Api.Services;
using VatCalculator.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddJsonConsole();

builder.Services
       .AddOptions<ApiOptions>()
       .Bind(builder.Configuration.GetSection("Api"))
       .ValidateDataAnnotations()
       .ValidateOnStart();

var version = GetVersion(builder);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
#if DEBUG
builder.Services.AddSwaggerDocumentation(version);
builder.Services.AddOpenApiDocumentation();
#endif

builder.Services.AddHealthChecks();

builder.Services.AddVatStrategyServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // Documentation tools
    app.UseSwaggerDocumentation(version);
    app.UseScalarDocumentation(version);
    app.MapOpenApi();
    app.MapOpenApi("/openapi/{documentName}.yaml");
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapErrorHandlingEndpoints();
app.MapVatCalculationEndpoints();
app.MapVatStrategyEndpoints();

app.MapFallbackToFile("index.html");
app.MapHealthChecks(Constants.Health);
app.Run();

[ExcludeFromCodeCoverage]
static string GetVersion(WebApplicationBuilder builder)
{
    return builder.Configuration["Api:Version"] ?? "v1";
}