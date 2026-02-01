using VatCalculator.Api.Interfaces;
using VatCalculator.Shared;

namespace VatCalculator.Api.Services;

public abstract class VatStrategyBase(decimal[] validRates) : IVatStrategy
{
    public abstract string CountryCode { get; }

    public abstract string CountryName { get; }

    public IReadOnlyList<decimal> ValidRates { get; } = validRates ?? throw new ArgumentNullException(nameof(validRates));

    protected virtual decimal Round(decimal value) => Math.Round(value, 2);

    private void ValidateRate(decimal rate)
    {
        if (!ValidRates.Contains(rate))
        {
            throw new ArgumentException($"Invalid VAT rate for {CountryCode}. Valid rates are: {String.Join(", ", ValidRates)}%");
        }
    }

    public VatCalculationResponse CalculateFromNet(VatCalculationFromNetRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateRate(request.VatRate);

        var rateMultiplier = request.VatRate / 100m;
        var vatAmount = request.Net * rateMultiplier;
        var gross = request.Net + vatAmount;

        return new VatCalculationResponse(Round(request.Net), Round(gross), Round(vatAmount), request.VatRate);
    }

    public VatCalculationResponse CalculateFromGross(VatCalculationFromGrossRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateRate(request.VatRate);

        var rateMultiplier = request.VatRate / 100m;
        var net = request.Gross / (1 + rateMultiplier);
        var vatAmount = request.Gross - net;

        return new VatCalculationResponse(Round(net), Round(request.Gross), Round(vatAmount), request.VatRate);
    }

    public VatCalculationResponse CalculateFromVat(VatCalculationFromVatRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateRate(request.VatRate);

        var rateMultiplier = request.VatRate / 100m;
        if (rateMultiplier == 0)
        {
            if (request.VatAmount > 0)
            {
                throw new ArgumentException("Cannot calculate amounts from VAT if VAT rate is 0%.");
            }
            return new VatCalculationResponse(0, 0, 0, 0);
        }

        var net = request.VatAmount / rateMultiplier;
        var gross = net + request.VatAmount;

        return new VatCalculationResponse(Round(net), Round(gross), Round(request.VatAmount), request.VatRate);
    }
}
