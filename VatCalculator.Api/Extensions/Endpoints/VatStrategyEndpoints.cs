using Microsoft.AspNetCore.Mvc;
using VatCalculator.Api.Interfaces;
using VatCalculator.Shared;

namespace VatCalculator.Api.Extensions.Endpoints;

public static class VatStrategyEndpoints
{
    public static void MapVatStrategyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{Constants.VatBase}/strategies")
                       .WithTags("VAT Strategies");

        group.MapGet(String.Empty, GetStrategies)
             .WithName("GetVatStrategies")
             .WithSummary("Lists available VAT strategies")
             .WithDescription("Returns all supported VAT calculation strategies. Each strategy contains the country code, display name and valid VAT rates.")
             .Produces<IEnumerable<VatStrategyInfo>>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/{countryCode}", GetStrategyByCountry)
             .WithName("GetVatStrategy")
             .WithSummary("Get VAT strategy by country code")
             .WithDescription("Returns a single VAT strategy by country code or 404 if not found.")
             .Produces<VatStrategyInfo>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static IResult GetStrategies(
        [FromServices] IEnumerable<IVatStrategy> strategies,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken ct)
    {
        var logger = loggerFactory.CreateLogger("VatStrategyEndpoints");
        try
        {
            var infos = strategies
                .Select(s => new VatStrategyInfo
                {
                    CountryCode = s.CountryCode,
                    Name = s.CountryName,
                    Rates = [.. s.ValidRates]
                }).ToList();

            return Results.Ok(infos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get VAT strategies");
            return Results.Problem(title: "Failed to fetch VAT strategies", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static IResult GetStrategyByCountry(
        string countryCode,
        [FromServices] IEnumerable<IVatStrategy> strategies,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken ct)
    {
        var logger = loggerFactory.CreateLogger("VatStrategyEndpoints");
        try
        {
            var found = strategies
                .FirstOrDefault(s => s.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

            if (found is null)
            {
                return Results.NotFound(new { Error = $"Country code '{countryCode}' is not supported." });
            }

            var info = new VatStrategyInfo
            {
                CountryCode = found.CountryCode,
                Name = found.CountryName,
                Rates = [.. found.ValidRates]
            };
            return Results.Ok(info);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get VAT strategy for {country}", countryCode);
            return Results.Problem(title: "Failed to fetch VAT strategy", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}