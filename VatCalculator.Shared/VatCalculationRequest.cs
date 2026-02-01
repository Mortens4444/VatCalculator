using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VatCalculator.Shared;

/// <summary>
/// Data model for the VAT calculation request.
/// </summary>
public abstract class VatCalculationRequest
{
    /// <summary>
    /// VAT rate as a percentage (e.g., 20, 10).
    /// </summary>
    [DefaultValue(typeof(decimal), "20")]
    public decimal VatRate { get; set; }

    /// <summary>
    /// Country code (e.g., "AT", "HU"). Default is AT.
    /// </summary>
    [DefaultValue("AT")]
    [StringLength(2)]
    public string CountryCode { get; set; } = "AT";
}
