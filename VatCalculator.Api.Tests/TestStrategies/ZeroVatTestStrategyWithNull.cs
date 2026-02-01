namespace VatCalculator.Api.Tests.TestStrategies;

internal class ZeroVatTestStrategyWithNull() : Services.VatStrategyBase(null!)
{
    public override string CountryCode => "X";

    public override string CountryName => "X";
}
