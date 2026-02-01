using VatCalculator.Shared;

namespace VatCalculator.Api.Interfaces;

public interface IVatStrategy
{
    string CountryName { get; }
    
    string CountryCode { get; }

    IReadOnlyList<decimal> ValidRates { get; }

    VatCalculationResponse CalculateFromNet(VatCalculationFromNetRequest request);

    VatCalculationResponse CalculateFromGross(VatCalculationFromGrossRequest request);

    VatCalculationResponse CalculateFromVat(VatCalculationFromVatRequest request);
}
