using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
/// Result of a wallet-to-wallet transfer operation.
/// </summary>
public sealed class WalletTransferResult
{
    /// <summary>
    /// Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; set; }
    
    /// <summary>
    /// Gets or sets whether the transfer was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets the transfer status message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
    /// <summary>
    /// Gets or sets the transaction ID.
    /// </summary>
    [JsonPropertyName("transactionId")]
    public Guid? TransactionId { get; set; }
}