namespace VatCalculator.Api.Services
{
    public class AustrianVatStrategy : VatStrategyBase
    {
        public override string CountryCode => "AT";

        public override string CountryName => "Austria";

        private static readonly decimal[] rates = [10m, 13m, 20m];

        public AustrianVatStrategy() : base(rates) { }
    }
}
