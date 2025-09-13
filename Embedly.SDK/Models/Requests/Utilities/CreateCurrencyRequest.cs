using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Utilities;

/// <summary>
/// Request model for adding a new currency.
/// </summary>
public sealed record CreateCurrencyRequest
{
    /// <summary>
    /// Gets or sets the currency code (e.g., NGN, USD).
    /// </summary>
    [Required(ErrorMessage = "Currency code is required")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters")]
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the currency name.
    /// </summary>
    [Required(ErrorMessage = "Currency name is required")]
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the currency symbol.
    /// </summary>
    [Required(ErrorMessage = "Currency symbol is required")]
    [JsonPropertyName("symbol")]
    public string Symbol { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether the currency is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; } = true;
    
    /// <summary>
    /// Gets or sets the number of decimal places.
    /// </summary>
    [Range(0, 10, ErrorMessage = "Decimal places must be between 0 and 10")]
    [JsonPropertyName("decimalPlaces")]
    public int DecimalPlaces { get; init; } = 2;
}