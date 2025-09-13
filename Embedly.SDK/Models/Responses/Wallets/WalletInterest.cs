using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
/// Represents monthly wallet interest details.
/// </summary>
public sealed class WalletInterest
{
    /// <summary>
    /// Gets or sets the wallet ID.
    /// </summary>
    [JsonPropertyName("walletId")]
    public string WalletId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the monthly interest amount.
    /// </summary>
    [JsonPropertyName("monthlyInterest")]
    public decimal MonthlyInterest { get; set; }
    
    /// <summary>
    /// Gets or sets the interest rate applied.
    /// </summary>
    [JsonPropertyName("interestRate")]
    public decimal InterestRate { get; set; }
    
    /// <summary>
    /// Gets or sets the wallet balance used for interest calculation.
    /// </summary>
    [JsonPropertyName("walletBalance")]
    public decimal WalletBalance { get; set; }
    
    /// <summary>
    /// Gets or sets the currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";
    
    /// <summary>
    /// Gets or sets the calculation period (month/year).
    /// </summary>
    [JsonPropertyName("period")]
    public string Period { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the interest calculation date.
    /// </summary>
    [JsonPropertyName("calculatedAt")]
    public DateTime CalculatedAt { get; set; }
}