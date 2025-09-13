using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Products;

/// <summary>
/// Represents a product in the Embedly system.
/// </summary>
public sealed class Product
{
    /// <summary>
    /// Gets or sets the unique product identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
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
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product status.
    /// </summary>
    [JsonPropertyName("status")]
    public ProductStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets whether the product is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the product configuration.
    /// </summary>
    [JsonPropertyName("config")]
    public Dictionary<string, object?>? Config { get; set; }
    
    /// <summary>
    /// Gets or sets the product limits.
    /// </summary>
    [JsonPropertyName("limits")]
    public List<ProductLimit>? Limits { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the product was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the product was last updated.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset? UpdatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets additional metadata for the product.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object?>? Metadata { get; set; }
}

/// <summary>
/// Product status enumeration.
/// </summary>
public enum ProductStatus
{
    /// <summary>
    /// Product is active and available.
    /// </summary>
    [JsonPropertyName("active")]
    Active,
    
    /// <summary>
    /// Product is inactive.
    /// </summary>
    [JsonPropertyName("inactive")]
    Inactive,
    
    /// <summary>
    /// Product is in development.
    /// </summary>
    [JsonPropertyName("development")]
    Development,
    
    /// <summary>
    /// Product is deprecated.
    /// </summary>
    [JsonPropertyName("deprecated")]
    Deprecated
}

/// <summary>
/// Represents a product limit.
/// </summary>
public sealed class ProductLimit
{
    /// <summary>
    /// Gets or sets the limit type.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the limit value.
    /// </summary>
    [JsonPropertyName("value")]
    public long Value { get; set; }
    
    /// <summary>
    /// Gets or sets the limit period.
    /// </summary>
    [JsonPropertyName("period")]
    public string? Period { get; set; }
    
    /// <summary>
    /// Gets or sets the currency for monetary limits.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}