using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
/// Represents wallet details for payout operations.
/// </summary>
public sealed class WalletDetails
{
    /// <summary>
    /// Gets or sets the wallet ID.
    /// </summary>
    [JsonPropertyName("walletId")]
    public Guid WalletId { get; set; }
    
    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the available balance.
    /// </summary>
    [JsonPropertyName("availableBalance")]
    public double AvailableBalance { get; set; }
    
    /// <summary>
    /// Gets or sets the ledger balance.
    /// </summary>
    [JsonPropertyName("ledgerBalance")]
    public double LedgerBalance { get; set; }
    
    /// <summary>
    /// Gets or sets the currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";
    
    /// <summary>
    /// Gets or sets the wallet status.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}