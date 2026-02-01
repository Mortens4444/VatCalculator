using System.ComponentModel;

namespace VatCalculator.Shared;

public class VatCalculationFromVatRequest : VatCalculationRequest
{
    /// <summary>
    /// VAT amount.
    /// </summary>
    [DefaultValue(typeof(decimal), "20")]
    public decimal VatAmount { get; set; }
}
