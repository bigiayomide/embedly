using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for closing a wallet.
/// </summary>
public sealed record CloseWalletRequest
{
    /// <summary>
    ///     Gets or sets the wallet account number to close.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the reason for closing the wallet.
    /// </summary>
    [Required(ErrorMessage = "Closure reason is required")]
    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;
}