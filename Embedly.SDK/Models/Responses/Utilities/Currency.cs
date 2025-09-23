using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Utilities;

/// <summary>
///     Represents a currency in the Embedly system.
/// </summary>
public sealed class Currency
{
    /// <summary>
    ///     Gets or sets the number of decimal places.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the currency code (e.g., NGN, USD).
    /// </summary>
    [JsonPropertyName("shortName")]
    public string ShortName { get; set; } = string.Empty;

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
}