using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.ProductLimits;

/// <summary>
/// Request model for creating a new product limit.
/// </summary>
public sealed class CreateProductLimitRequest
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [Required(ErrorMessage = "Product ID is required")]
    [JsonPropertyName("productId")]
    public string ProductId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the limit type.
    /// </summary>
    [Required(ErrorMessage = "Limit type is required")]
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the limit value.
    /// </summary>
    [Required(ErrorMessage = "Limit value is required")]
    [Range(0, long.MaxValue, ErrorMessage = "Limit value must be non-negative")]
    [JsonPropertyName("value")]
    public long Value { get; set; }
    
    /// <summary>
    /// Gets or sets the limit period (e.g., "daily", "monthly", "yearly").
    /// </summary>
    [JsonPropertyName("period")]
    public string? Period { get; set; }
    
    /// <summary>
    /// Gets or sets the currency for monetary limits.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; } = "NGN";
    
    /// <summary>
    /// Gets or sets whether the limit is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the limit description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}