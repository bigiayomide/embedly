using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for funding a wallet.
/// </summary>
public sealed record FundWalletRequest
{
    /// <summary>
    ///     Gets or sets the wallet ID to fund.
    /// </summary>
    [Required(ErrorMessage = "Wallet ID is required")]
    [JsonPropertyName("walletId")]
    public string WalletId { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the amount to fund.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }

    /// <summary>
    ///     Gets or sets the currency code.
    /// </summary>
    [Required(ErrorMessage = "Currency is required")]
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }

    /// <summary>
    ///     Gets or sets the transaction description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    ///     Gets or sets the payment method.
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; init; }

    /// <summary>
    ///     Gets or sets additional metadata for the transaction.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object?>? Metadata { get; init; }
}