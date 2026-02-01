using VatCalculator.Api.Tests.TestStrategies;
using VatCalculator.Api.Tests.UnitTests;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.IntegrationTests;

[TestFixture]
public class VatStrategyBaseSpecialCaseTests : VatStrategyTestsBase<ZeroVatTestStrategy>
{
    protected override string CountryCode => "ZZ";
    protected override decimal[] ValidRates => [0m, 20m];
    protected override decimal InvalidRate => 99m;

    [Test]
    public void CalculateFromVat_WithZeroRateAndZeroAmount_ReturnsZeroResponse()
    {
        // Ez lefedi a: if (rateMultiplier == 0) { if (request.VatAmount > 0) is false } ágat
        var request = new VatCalculationFromVatRequest
        {
            VatAmount = 0,
            VatRate = 0,
            CountryCode = CountryCode
        };

        var result = Strategy.CalculateFromVat(request);

        Assert.Multiple(() =>
        {
            Assert.That(result.Net, Is.EqualTo(0));
            Assert.That(result.Gross, Is.EqualTo(0));
            Assert.That(result.VatAmount, Is.EqualTo(0));
            Assert.That(result.VatRate, Is.EqualTo(0));
        });
    }

    [Test]
    public void CalculateFromVat_WithZeroRateAndPositiveAmount_ThrowsArgumentException()
    {
        // Ez lefedi a: if (rateMultiplier == 0) { if (request.VatAmount > 0) is true } ágat
        var request = new VatCalculationFromVatRequest
        {
            VatAmount = 10.5m,
            VatRate = 0,
            CountryCode = CountryCode
        };

        var ex = Assert.Throws<ArgumentException>(() => Strategy.CalculateFromVat(request));
        Assert.That(ex!.Message, Does.Contain("Cannot calculate amounts from VAT if VAT rate is 0%"));
    }

    [Test]
    public void Constructor_WithNullRates_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ZeroVatTestStrategyWithNull());
    }
}