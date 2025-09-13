using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Utilities;

/// <summary>
/// Represents a country in the Embedly system.
/// </summary>
public sealed class Country
{
    /// <summary>
    /// Gets or sets the country ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the country name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the country code (ISO 2-letter).
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the country code (ISO 3-letter).
    /// </summary>
    [JsonPropertyName("code3")]
    public string? Code3 { get; set; }
    
    /// <summary>
    /// Gets or sets whether the country is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the country dial code.
    /// </summary>
    [JsonPropertyName("dialCode")]
    public string? DialCode { get; set; }
}