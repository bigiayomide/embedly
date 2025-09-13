using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Payout;

/// <summary>
/// Request model for payout profile organization based on PayoutRequestDto schema.
/// </summary>
public sealed class PayoutRequestDto
{
    /// <summary>
    /// Gets or sets the wallet ID.
    /// </summary>
    [Required(ErrorMessage = "Wallet ID is required")]
    [JsonPropertyName("walletId")]
    public Guid WalletId { get; set; }
}