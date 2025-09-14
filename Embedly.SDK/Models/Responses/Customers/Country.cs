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
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the country name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country code.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the ISO code.
    /// </summary>
    [JsonPropertyName("isoCode")]
    public string IsoCode { get; set; } = string.Empty;
}