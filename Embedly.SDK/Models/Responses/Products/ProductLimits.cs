using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Products;

/// <summary>
/// Represents product limits for transactions and operations.
/// </summary>
public sealed class ProductLimits
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [JsonPropertyName("productId")]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the currency ID.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public Guid CurrencyId { get; set; }
    
    /// <summary>
    /// Gets or sets the customer ID (if customer-specific).
    /// </summary>
    [JsonPropertyName("customerId")]
    public string? CustomerId { get; set; }
    
    /// <summary>
    /// Gets or sets the daily transaction limit.
    /// </summary>
    [JsonPropertyName("dailyTransactionLimit")]
    public decimal DailyTransactionLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the monthly transaction limit.
    /// </summary>
    [JsonPropertyName("monthlyTransactionLimit")]
    public decimal MonthlyTransactionLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum single transaction amount.
    /// </summary>
    [JsonPropertyName("maxSingleTransactionAmount")]
    public decimal MaxSingleTransactionAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the minimum single transaction amount.
    /// </summary>
    [JsonPropertyName("minSingleTransactionAmount")]
    public decimal MinSingleTransactionAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the daily transaction count limit.
    /// </summary>
    [JsonPropertyName("dailyTransactionCount")]
    public int DailyTransactionCount { get; set; }
    
    /// <summary>
    /// Gets or sets the monthly transaction count limit.
    /// </summary>
    [JsonPropertyName("monthlyTransactionCount")]
    public int MonthlyTransactionCount { get; set; }
    
    /// <summary>
    /// Gets or sets whether these are default limits.
    /// </summary>
    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; set; }
    
    /// <summary>
    /// Gets or sets when these limits were created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets when these limits were last updated.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}