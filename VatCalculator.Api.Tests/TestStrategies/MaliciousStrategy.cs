using VatCalculator.Api.Interfaces;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.TestStrategies;

internal class MaliciousStrategy : IVatStrategy
{
    public string CountryCode => "ERR";

    public string CountryName => "Error Land";

    public IReadOnlyList<decimal> ValidRates => [27];

    public VatCalculationResponse CalculateFromNet(VatCalculationFromNetRequest r)
        => throw new ArgumentException("Test invalid rate message");

    public VatCalculationResponse CalculateFromGross(VatCalculationFromGrossRequest r)
        => throw new Exception("Unexpected system failure");

    public VatCalculationResponse CalculateFromVat(VatCalculationFromVatRequest r)
        => throw new NotImplementedException();
}