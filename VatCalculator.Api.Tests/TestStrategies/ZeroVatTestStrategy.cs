namespace VatCalculator.Api.Tests.TestStrategies;

public class ZeroVatTestStrategy : Services.VatStrategyBase
{
    public ZeroVatTestStrategy() : base([0m, 20m]) { }

    public override string CountryCode => "ZZ";

    public override string CountryName => "Paradise City - Zero Tax Land";
}