using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for wallet-to-wallet transfers based on WalletToWalletTransferDtoV2.
/// </summary>
public sealed record WalletToWalletTransferRequest
{
    /// <summary>
    ///     Gets or sets the source account number.
    /// </summary>
    [JsonPropertyName("fromAccount")]
    public string? FromAccount { get; init; }

    /// <summary>
    ///     Gets or sets the destination account number.
    /// </summary>
    [JsonPropertyName("toAccount")]
    public string? ToAccount { get; init; }

    /// <summary>
    ///     Gets or sets the transfer amount.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [JsonPropertyName("amount")]
    public double Amount { get; init; }

    /// <summary>
    ///     Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; init; }

    /// <summary>
    ///     Gets or sets the transfer remarks.
    /// </summary>
    [JsonPropertyName("remarks")]
    public string? Remarks { get; init; }

    /// <summary>
    ///     Gets or sets the transaction type ID.
    /// </summary>
    [JsonPropertyName("transactionTypeId")]
    public int? TransactionTypeId { get; init; }
}