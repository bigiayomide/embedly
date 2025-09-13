using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Payout;

/// <summary>
/// Request model for adding global payout limits.
/// </summary>
public sealed record AddGlobalPayoutLimitRequest
{
    /// <summary>
    /// Gets or sets the currency ID.
    /// </summary>
    [Required(ErrorMessage = "Currency ID is required")]
    [JsonPropertyName("currencyId")]
    public Guid CurrencyId { get; init; }
    
    /// <summary>
    /// Gets or sets the daily transaction limit.
    /// </summary>
    [Required(ErrorMessage = "Daily transaction limit is required")]
    [JsonPropertyName("dailyTransactionLimit")]
    public double DailyTransactionLimit { get; init; }
    
    /// <summary>
    /// Gets or sets the daily transaction count.
    /// </summary>
    [Required(ErrorMessage = "Daily transaction count is required")]
    [JsonPropertyName("dailyTransactionCount")]
    public int DailyTransactionCount { get; init; }
    
    /// <summary>
    /// Gets or sets the daily transaction limit status.
    /// </summary>
    [JsonPropertyName("dailyTransactionLimitStatus")]
    public bool DailyTransactionLimitStatus { get; init; }
    
    /// <summary>
    /// Gets or sets the daily transaction count status.
    /// </summary>
    [JsonPropertyName("dailyTransactionCountStatus")]
    public bool DailyTransactionCountStatus { get; init; }
}