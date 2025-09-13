using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
/// Represents a global payout limit.
/// </summary>
public sealed class GlobalPayoutLimit
{
    /// <summary>
    /// Gets or sets the global limit ID.
    /// </summary>
    [JsonPropertyName("globalLimitId")]
    public Guid GlobalLimitId { get; set; }
    
    /// <summary>
    /// Gets or sets the currency ID.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public Guid CurrencyId { get; set; }
    
    /// <summary>
    /// Gets or sets the daily transaction limit.
    /// </summary>
    [JsonPropertyName("dailyTransactionLimit")]
    public double DailyTransactionLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the daily transaction count.
    /// </summary>
    [JsonPropertyName("dailyTransactionCount")]
    public int DailyTransactionCount { get; set; }
    
    /// <summary>
    /// Gets or sets the daily transaction limit status.
    /// </summary>
    [JsonPropertyName("dailyTransactionLimitStatus")]
    public bool DailyTransactionLimitStatus { get; set; }
    
    /// <summary>
    /// Gets or sets the daily transaction count status.
    /// </summary>
    [JsonPropertyName("dailyTransactionCountStatus")]
    public bool DailyTransactionCountStatus { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the last updated date.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}