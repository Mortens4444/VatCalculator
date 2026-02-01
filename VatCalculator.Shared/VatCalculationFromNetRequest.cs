using System.ComponentModel;

namespace VatCalculator.Shared;

public class VatCalculationFromNetRequest : VatCalculationRequest
{
    /// <summary>
    /// Net amount.
    /// </summary>
    [DefaultValue(typeof(decimal), "100")]
    public decimal Net { get; set; }
}
