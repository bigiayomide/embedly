using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Cards;

/// <summary>
/// Request model for unblocking an Afrigo card based on CardUnblockRequestDto schema.
/// </summary>
public sealed record UnblockCardRequest
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public Guid CustomerId { get; init; }
    
    /// <summary>
    /// Gets or sets the wallet ID.
    /// </summary>
    [Required(ErrorMessage = "Wallet ID is required")]
    [JsonPropertyName("walletId")]
    public Guid WalletId { get; init; }
    
    /// <summary>
    /// Gets or sets the card number.
    /// </summary>
    [JsonPropertyName("cardNumber")]
    public string? CardNumber { get; init; }
}