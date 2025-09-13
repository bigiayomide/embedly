using System;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Common;

namespace Embedly.SDK.Models.Responses.ProductLimits;

/// <summary>
/// Represents a product limit in the Embedly system.
/// </summary>
public sealed class ProductLimit
{
    /// <summary>
    /// Gets or sets the unique product limit identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [JsonPropertyName("productId")]
    public string ProductId { get; set; } = string.Empty;
    
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
    
    /// <summary>
    /// Gets or sets whether the limit is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the limit description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets the current usage against this limit.
    /// </summary>
    [JsonPropertyName("currentUsage")]
    public long CurrentUsage { get; set; }
    
    /// <summary>
    /// Gets or sets the remaining limit.
    /// </summary>
    [JsonPropertyName("remaining")]
    public long Remaining { get; set; }
    
    /// <summary>
    /// Gets or sets the limit status.
    /// </summary>
    [JsonPropertyName("status")]
    public ProductLimitStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the limit was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the limit was last updated.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset? UpdatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the next reset date for periodic limits.
    /// </summary>
    [JsonPropertyName("nextResetAt")]
    public DateTimeOffset? NextResetAt { get; set; }
    
    /// <summary>
    /// Gets the limit value as a Money object (for monetary limits).
    /// </summary>
    public Money? GetLimitAsMoney()
    {
        if (string.IsNullOrWhiteSpace(Currency))
            return null;
            
        return new Money(Value, Currency);
    }
    
    /// <summary>
    /// Gets the current usage as a Money object (for monetary limits).
    /// </summary>
    public Money? GetCurrentUsageAsMoney()
    {
        if (string.IsNullOrWhiteSpace(Currency))
            return null;
            
        return new Money(CurrentUsage, Currency);
    }
    
    /// <summary>
    /// Gets the remaining limit as a Money object (for monetary limits).
    /// </summary>
    public Money? GetRemainingAsMoney()
    {
        if (string.IsNullOrWhiteSpace(Currency))
            return null;
            
        return new Money(Remaining, Currency);
    }
    
    /// <summary>
    /// Gets the usage percentage (0-100).
    /// </summary>
    public decimal GetUsagePercentage()
    {
        if (Value == 0) return 0;
        return Math.Round((decimal)CurrentUsage / Value * 100, 2);
    }
}

/// <summary>
/// Product limit status enumeration.
/// </summary>
public enum ProductLimitStatus
{
    /// <summary>
    /// Limit is active and being enforced.
    /// </summary>
    [JsonPropertyName("active")]
    Active,
    
    /// <summary>
    /// Limit is inactive.
    /// </summary>
    [JsonPropertyName("inactive")]
    Inactive,
    
    /// <summary>
    /// Limit has been exceeded.
    /// </summary>
    [JsonPropertyName("exceeded")]
    Exceeded,
    
    /// <summary>
    /// Limit is approaching threshold.
    /// </summary>
    [JsonPropertyName("warning")]
    Warning
}