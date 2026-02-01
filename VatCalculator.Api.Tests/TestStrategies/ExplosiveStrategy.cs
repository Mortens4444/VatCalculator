using VatCalculator.Api.Interfaces;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.TestStrategies;

internal class ExplosiveStrategy : IVatStrategy
{
    // A LINQ FirstOrDefault vagy Select azonnal meghívja ezeket, kiváltva a catch ágat
    public string CountryCode => throw new Exception("Database connection failure simulation");
    public string CountryName => throw new Exception("Property access error");
    public IReadOnlyList<decimal> ValidRates => throw new Exception("Rates unavailable");

    public VatCalculationResponse CalculateFromNet(VatCalculationFromNetRequest request) => throw new NotImplementedException();
    public VatCalculationResponse CalculateFromGross(VatCalculationFromGrossRequest request) => throw new NotImplementedException();
    public VatCalculationResponse CalculateFromVat(VatCalculationFromVatRequest request) => throw new NotImplementedException();
}
