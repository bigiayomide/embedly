using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for wallet-to-wallet transfers.
/// </summary>
public sealed record WalletToWalletTransferRequest
{
    /// <summary>
    ///     Gets or sets the source account number.
    /// </summary>
    [Required(ErrorMessage = "FromAccount is required")]
    [JsonPropertyName("fromAccount")]
    public string FromAccount { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the destination account number.
    /// </summary>
    [Required(ErrorMessage = "ToAccount is required")]
    [JsonPropertyName("toAccount")]
    public string ToAccount { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the transfer amount.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }

    /// <summary>
    ///     Gets or sets the unique transaction reference (recommended: GUID or 10+ character string).
    /// </summary>
    [Required(ErrorMessage = "TransactionReference is required")]
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets optional remarks or notes for the transaction.
    /// </summary>
    [JsonPropertyName("remarks")]
    public string? Remarks { get; init; }
}