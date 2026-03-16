using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Utilities;

/// <summary>
///     Represents a country in the Embedly system.
/// </summary>
public sealed class Country
{
    /// <summary>
    ///     Gets or sets the country ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country code (ISO 2-letter).
    /// </summary>
    [JsonPropertyName("countryCodeTwo")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country code (ISO 3-letter).
    /// </summary>
    [JsonPropertyName("countryCodeThree")]
    public string? Code3 { get; set; }
}
