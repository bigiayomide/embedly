using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for restricting a wallet.
/// </summary>
public sealed record RestrictWalletRequest
{
    /// <summary>
    ///     Gets or sets the wallet ID to restrict.
    /// </summary>
    [Required(ErrorMessage = "Wallet ID is required")]
    [JsonPropertyName("walletId")]
    public Guid WalletId { get; init; }

    /// <summary>
    ///     Gets or sets the restriction type ID.
    /// </summary>
    [Required(ErrorMessage = "Restriction type ID is required")]
    [JsonPropertyName("walletRestrictionId")]
    public Guid WalletRestrictionId { get; init; }

    /// <summary>
    ///     Gets or sets the restriction reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }
}