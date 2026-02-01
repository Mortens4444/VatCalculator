using VatCalculator.Api.Services;

namespace VatCalculator.Api.Tests.UnitTests;

[TestFixture]
public class AustrianVatStrategyTests : VatStrategyTestsBase<AustrianVatStrategy>
{
    protected override string CountryCode => "AT";

    protected override decimal[] ValidRates => [10m, 13m, 20m];

    protected override decimal InvalidRate => 25m;

    [TestCaseSource(nameof(GetNetCases))]
    public void Calculate_FromNet_ReturnsCorrectValues(decimal net, decimal rate, decimal expectedGross, decimal expectedVat)
    {
        AssertNetCalculation(net, rate, expectedGross, expectedVat);
    }

    protected static IEnumerable<TestCaseData> GetNetCases()
    {
        // Format: net, rate, expectedGross, expectedVat

        // 10% VAT rate tests (reduced rate)
        yield return new TestCaseData(100m, 10m, 110m, 10m)
            .SetName("Net=100, Rate=10% → Gross=110, VAT=10");

        // 13% VAT rate tests (reduced rate)
        yield return new TestCaseData(100m, 13m, 113m, 13m)
            .SetName("Net=100, Rate=13% → Gross=113, VAT=13");

        // 20% VAT rate tests (standard rate)
        yield return new TestCaseData(100m, 20m, 120m, 20m)
            .SetName("Net=100, Rate=20% → Gross=120, VAT=20");

        yield return new TestCaseData(200m, 20m, 240m, 40m)
            .SetName("Net=200, Rate=20% → Gross=240, VAT=40");

        yield return new TestCaseData(50m, 20m, 60m, 10m)
            .SetName("Net=50, Rate=20% → Gross=60, VAT=10");
    }
}