using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
/// Status of a wallet-to-wallet transfer.
/// </summary>
public sealed class WalletTransferStatus
{
    /// <summary>
    /// Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; set; }
    
    /// <summary>
    /// Gets or sets the transfer status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    
    /// <summary>
    /// Gets or sets the status description.
    /// </summary>
    [JsonPropertyName("statusDescription")]
    public string? StatusDescription { get; set; }
    
    /// <summary>
    /// Gets or sets the transfer amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the source account.
    /// </summary>
    [JsonPropertyName("fromAccount")]
    public string? FromAccount { get; set; }
    
    /// <summary>
    /// Gets or sets the destination account.
    /// </summary>
    [JsonPropertyName("toAccount")]
    public string? ToAccount { get; set; }
    
    /// <summary>
    /// Gets or sets the transaction date.
    /// </summary>
    [JsonPropertyName("transactionDate")]
    public DateTime? TransactionDate { get; set; }
}