using VatCalculator.Api.Services;

namespace VatCalculator.Api.Tests.UnitTests;

[TestFixture]
public class HungarianVatStrategyTests
    : VatStrategyTestsBase<HungarianVatStrategy>
{
    protected override string CountryCode => "HU";

    protected override decimal[] ValidRates => [5m, 18m, 27m];

    protected override decimal InvalidRate => 20m;

    [TestCaseSource(nameof(GetNetCases))]
    public void Calculate_FromNet_ReturnsCorrectValues(decimal net, decimal rate, decimal expectedGross, decimal expectedVat)
    {
        AssertNetCalculation(net, rate, expectedGross, expectedVat);
    }

    protected static IEnumerable<TestCaseData> GetNetCases()
    {
        // Format: net, rate, expectedGross, expectedVat

        // 5% VAT rate tests
        yield return new TestCaseData(100m, 5m, 105m, 5m)
            .SetName("Net=100, Rate=5% → Gross=105, VAT=5");

        // 18% VAT rate tests
        yield return new TestCaseData(100m, 18m, 118m, 18m)
            .SetName("Net=100, Rate=18% → Gross=118, VAT=18");

        // 27% VAT rate tests (standard rate)
        yield return new TestCaseData(100m, 27m, 127m, 27m)
            .SetName("Net=100, Rate=27% → Gross=127, VAT=27");

        yield return new TestCaseData(200m, 27m, 254m, 54m)
            .SetName("Net=200, Rate=27% → Gross=254, VAT=54");

        yield return new TestCaseData(50m, 27m, 63.5m, 13.5m)
            .SetName("Net=50, Rate=27% → Gross=63.5, VAT=13.5");
    }
}