namespace VatCalculator.Api.Services
{
    public class HungarianVatStrategy : VatStrategyBase
    {
        public override string CountryCode => "HU";

        public override string CountryName => "Hungary";

        private static readonly decimal[] hungarianRates = [27m, 18m, 5m];

        //protected override decimal Round(decimal value) => Math.Round(value / 5m, MidpointRounding.AwayFromZero) * 5m;

        public HungarianVatStrategy() : base(hungarianRates) { }
    }
}
