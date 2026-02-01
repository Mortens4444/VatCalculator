using VatCalculator.Api.Interfaces;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.UnitTests;

public abstract class VatStrategyTestsBase<TStrategy>
    where TStrategy : IVatStrategy, new()
{
    protected IVatStrategy Strategy = null!;

    protected abstract string CountryCode { get; }
    protected abstract decimal[] ValidRates { get; }
    protected abstract decimal InvalidRate { get; }

    [SetUp]
    public void Setup()
    {
        Strategy = new TStrategy();
    }

    [Test]
    public void Calculate_WithInvalidRate_ThrowsException()
    {
        var request = new VatCalculationFromNetRequest
        {
            Net = 100,
            VatRate = InvalidRate,
            CountryCode = CountryCode
        };

        var ex = Assert.Throws<ArgumentException>(() => Strategy.CalculateFromNet(request));
        Assert.That(ex!.Message, Does.Contain("Invalid VAT rate"));
    }

    [Test]
    public void Calculate_WithNullRequest_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Strategy.CalculateFromNet(null!));
    }

    protected void AssertNetCalculation(decimal net, decimal rate, decimal expectedGross, decimal expectedVat)
    {
        var request = new VatCalculationFromNetRequest
        {
            Net = net,
            VatRate = rate,
            CountryCode = CountryCode
        };

        var result = Strategy.CalculateFromNet(request);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Gross, Is.EqualTo(expectedGross));
            Assert.That(result.VatAmount, Is.EqualTo(expectedVat));
        }
    }

    [Test]
    public void Calculate_FromGross_ReturnsCorrectValues()
    {
        var rate = ValidRates.Last();
        var gross = 120m;

        var request = new VatCalculationFromGrossRequest
        {
            Gross = gross,
            VatRate = rate,
            CountryCode = CountryCode
        };

        var result = Strategy.CalculateFromGross(request);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Net + result.VatAmount, Is.EqualTo(result.Gross));
        }
    }

    [Test]
    public void Calculate_FromVat_ReturnsCorrectValues()
    {
        var rate = ValidRates.Last();
        var vat = 20m;

        var request = new VatCalculationFromVatRequest
        {
            VatAmount = vat,
            VatRate = rate,
            CountryCode = CountryCode
        };

        var result = Strategy.CalculateFromVat(request);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Gross - result.Net, Is.EqualTo(result.VatAmount));
        }
    }
}