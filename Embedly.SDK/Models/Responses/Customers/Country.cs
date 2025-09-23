using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
///     Represents a country for customer operations.
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
    ///     Gets or sets the country code.
    /// </summary>
    [JsonPropertyName("countryCodeTwo")]
    public string CountryCodeTwo { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the ISO code.
    /// </summary>
    [JsonPropertyName("countryCodeThree")]
    public string CountryCodeThree { get; set; } = string.Empty;
}