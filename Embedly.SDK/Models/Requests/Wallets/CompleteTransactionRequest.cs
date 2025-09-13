using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
/// Request model for completing a wallet transaction.
/// </summary>
public sealed record CompleteTransactionRequest
{
    /// <summary>
    /// Gets or sets the transaction ID.
    /// </summary>
    [Required(ErrorMessage = "Transaction ID is required")]
    [JsonPropertyName("transactionId")]
    public Guid TransactionId { get; init; }
    
    /// <summary>
    /// Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; init; }
    
    /// <summary>
    /// Gets or sets completion remarks.
    /// </summary>
    [JsonPropertyName("remarks")]
    public string? Remarks { get; init; }
}