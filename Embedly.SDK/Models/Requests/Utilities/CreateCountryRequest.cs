using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Utilities;

/// <summary>
///     Request model for adding a new country.
/// </summary>
public sealed record CreateCountryRequest
{
    /// <summary>
    ///     Gets or sets the country name.
    /// </summary>
    [Required(ErrorMessage = "Country name is required")]
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country code (ISO 2-letter).
    /// </summary>
    [Required(ErrorMessage = "Country code is required")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be exactly 2 characters")]
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country code (ISO 3-letter).
    /// </summary>
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Country code 3 must be exactly 3 characters")]
    [JsonPropertyName("code3")]
    public string? Code3 { get; init; }

    /// <summary>
    ///     Gets or sets whether the country is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; } = true;

    /// <summary>
    ///     Gets or sets the country dial code.
    /// </summary>
    [JsonPropertyName("dialCode")]
    public string? DialCode { get; init; }
}