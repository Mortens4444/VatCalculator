namespace VatCalculator.Shared;

public record VatCalculationResponse(decimal Net, decimal Gross, decimal VatAmount, decimal VatRate);
