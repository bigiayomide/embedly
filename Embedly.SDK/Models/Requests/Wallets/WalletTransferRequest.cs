using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Common;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
/// Request model for wallet-to-wallet transfers.
/// </summary>
public sealed class WalletTransferRequest
{
    /// <summary>
    /// Gets or sets the source wallet ID.
    /// </summary>
    [Required(ErrorMessage = "Source wallet ID is required")]
    [JsonPropertyName("fromWalletId")]
    public string FromWalletId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the destination wallet ID.
    /// </summary>
    [Required(ErrorMessage = "Destination wallet ID is required")]
    [JsonPropertyName("toWalletId")]
    public string ToWalletId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the transfer amount in the smallest currency unit.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [JsonPropertyName("amount")]
    public long Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";
    
    /// <summary>
    /// Gets or sets the transfer description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; set; }
    
    /// <summary>
    /// Gets or sets additional metadata for the transfer.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object?>? Metadata { get; set; }
    
    /// <summary>
    /// Creates a transfer request using Money object.
    /// </summary>
    public static WalletTransferRequest Create(string fromWalletId, string toWalletId, Money amount, string? description = null)
    {
        return new WalletTransferRequest
        {
            FromWalletId = fromWalletId,
            ToWalletId = toWalletId,
            Amount = amount.Amount,
            Currency = amount.Currency,
            Description = description
        };
    }
}