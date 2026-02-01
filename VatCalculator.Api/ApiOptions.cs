using System.ComponentModel;

namespace VatCalculator.Api;

public sealed class ApiOptions
{
    [DefaultValue("v1")]
    public string Version { get; init; } = "v1";

    public string Title { get; init; } = String.Empty;
}
