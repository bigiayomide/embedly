using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Utilities;

/// <summary>
///     Represents a currency in the Embedly system.
/// </summary>
public sealed class Currency
{
    /// <summary>
    ///     Gets or sets the currency code (e.g., NGN, USD).
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the currency name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the currency symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets whether the currency is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    ///     Gets or sets the number of decimal places.
    /// </summary>
    [JsonPropertyName("decimalPlaces")]
    public int DecimalPlaces { get; set; } = 2;
}