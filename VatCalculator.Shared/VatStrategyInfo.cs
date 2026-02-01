using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VatCalculator.Shared;

/// <summary>
/// Describes an available VAT calculation strategy.
/// </summary>
public class VatStrategyInfo
{
    /// <summary>
    /// ISO country code (e.g., "HU", "AT").
    /// </summary>
    [Required]
    [StringLength(2)]
    [DefaultValue("AT")]
    public string CountryCode { get; set; } = "AT";

    /// <summary>
    /// Human-readable strategy name.
    /// </summary>
    [Required]
    [DefaultValue("Austria")]
    public string Name { get; set; } = "Austria";

    /// <summary>
    /// List of valid VAT rates for the country.
    /// </summary>
    [Required]
    public IReadOnlyList<decimal> Rates { get; set; } = [10m, 13m, 20m];
}
