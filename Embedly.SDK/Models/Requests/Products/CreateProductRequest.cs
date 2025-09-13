using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Products;

/// <summary>
/// Request model for creating a new product.
/// </summary>
public sealed class CreateProductRequest
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    [Required(ErrorMessage = "Product name is required")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets the product type.
    /// </summary>
    [Required(ErrorMessage = "Product type is required")]
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product configuration.
    /// </summary>
    [JsonPropertyName("config")]
    public Dictionary<string, object?>? Config { get; set; }
    
    /// <summary>
    /// Gets or sets whether the product is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Gets or sets additional metadata for the product.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object?>? Metadata { get; set; }
}