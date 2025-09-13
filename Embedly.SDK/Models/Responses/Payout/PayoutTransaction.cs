using System;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Common;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
/// Represents a payout transaction.
/// </summary>
public sealed class PayoutTransaction
{
    /// <summary>
    /// Gets or sets the transaction ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the amount in the smallest currency unit.
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";
    
    /// <summary>
    /// Gets or sets the recipient account number.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the recipient account name.
    /// </summary>
    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the bank code.
    /// </summary>
    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the bank name.
    /// </summary>
    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the transaction status.
    /// </summary>
    [JsonPropertyName("status")]
    public PayoutStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the transaction narration.
    /// </summary>
    [JsonPropertyName("narration")]
    public string? Narration { get; set; }
    
    /// <summary>
    /// Gets or sets the source wallet ID.
    /// </summary>
    [JsonPropertyName("sourceWalletId")]
    public string? SourceWalletId { get; set; }
    
    /// <summary>
    /// Gets or sets the transaction fee.
    /// </summary>
    [JsonPropertyName("fee")]
    public long Fee { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the transaction was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the transaction was processed.
    /// </summary>
    [JsonPropertyName("processedAt")]
    public DateTimeOffset? ProcessedAt { get; set; }
    
    /// <summary>
    /// Gets the amount as a Money object.
    /// </summary>
    public Money GetAmount() => new(Amount, Currency);
    
    /// <summary>
    /// Gets the fee as a Money object.
    /// </summary>
    public Money GetFee() => new(Fee, Currency);
}

/// <summary>
/// Payout transaction status enumeration.
/// </summary>
public enum PayoutStatus
{
    /// <summary>
    /// Payout is pending.
    /// </summary>
    [JsonPropertyName("pending")]
    Pending,
    
    /// <summary>
    /// Payout is processing.
    /// </summary>
    [JsonPropertyName("processing")]
    Processing,
    
    /// <summary>
    /// Payout completed successfully.
    /// </summary>
    [JsonPropertyName("successful")]
    Successful,
    
    /// <summary>
    /// Payout failed.
    /// </summary>
    [JsonPropertyName("failed")]
    Failed,
    
    /// <summary>
    /// Payout was reversed.
    /// </summary>
    [JsonPropertyName("reversed")]
    Reversed
}