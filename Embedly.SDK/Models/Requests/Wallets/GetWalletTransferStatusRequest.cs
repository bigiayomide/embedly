using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for getting wallet-to-wallet transfer status.
/// </summary>
public sealed record GetWalletTransferStatusRequest
{
    /// <summary>
    ///     Gets or sets the transaction reference to check status for.
    /// </summary>
    [Required(ErrorMessage = "Transaction reference is required")]
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; init; } = string.Empty;
}
