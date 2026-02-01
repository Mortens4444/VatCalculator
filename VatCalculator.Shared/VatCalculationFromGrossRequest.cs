using System.ComponentModel;

namespace VatCalculator.Shared;

public class VatCalculationFromGrossRequest : VatCalculationRequest
{
    /// <summary>
    /// Gross amount.
    /// </summary>
    [DefaultValue(typeof(decimal), "120")]
    public decimal Gross { get; set; }
}
