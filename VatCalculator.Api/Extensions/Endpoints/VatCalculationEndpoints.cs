using Microsoft.AspNetCore.Mvc;
using VatCalculator.Api.Interfaces;
using VatCalculator.Shared;

namespace VatCalculator.Api.Extensions.Endpoints;

public static class VatCalculationEndpoints
{
    public static void MapVatCalculationEndpoints(this IEndpointRouteBuilder app)
    {
        var calcGroup = app.MapGroup($"{Constants.VatBase}/calculate")
            .WithTags("VAT Calculation");

        calcGroup.MapPost("/from-net", FromNet)
            .WithSummary("Calculate from Net amount");

        calcGroup.MapPost("/from-gross", FromGross)
            .WithSummary("Calculate from Gross amount");

        calcGroup.MapPost("/from-vat", FromVat)
            .WithSummary("Calculate from VAT amount");
    }

    private static IResult FromNet(
        [FromBody] VatCalculationFromNetRequest request,
        [FromServices] IEnumerable<IVatStrategy> strategies)
        => Execute(request, strategies, (strat, r) => strat.CalculateFromNet(r));

    private static IResult FromGross(
        [FromBody] VatCalculationFromGrossRequest request,
        [FromServices] IEnumerable<IVatStrategy> strategies)
        => Execute(request, strategies, (strat, r) => strat.CalculateFromGross(r));

    private static IResult FromVat(
        [FromBody] VatCalculationFromVatRequest request,
        [FromServices] IEnumerable<IVatStrategy> strategies)
        => Execute(request, strategies, (strat, r) => strat.CalculateFromVat(r));

    private static IResult Execute<TRequest>(
            TRequest request,
            IEnumerable<IVatStrategy> strategies,
            Func<IVatStrategy, TRequest, VatCalculationResponse> calcFunc)
            where TRequest : VatCalculationRequest
    {
        var strategy = strategies.FirstOrDefault(s =>
            s.CountryCode.Equals(request.CountryCode, StringComparison.OrdinalIgnoreCase));

        if (strategy == null)
        {
            return Results.BadRequest(new { Error = $"Country code '{request.CountryCode}' is not supported." });
        }

        try
        {
            var result = calcFunc(strategy, request);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(new
            {
                Error = "An unexpected error occurred.",
#if DEBUG
                Details = ex.ToString()
#endif
            });
        }
    }
}