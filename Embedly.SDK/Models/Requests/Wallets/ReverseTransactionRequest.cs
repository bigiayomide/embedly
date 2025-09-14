using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for reversing a wallet transaction.
/// </summary>
public sealed record ReverseTransactionRequest
{
    /// <summary>
    ///     Gets or sets the transaction ID to reverse.
    /// </summary>
    [Required(ErrorMessage = "Transaction ID is required")]
    [JsonPropertyName("transactionId")]
    public string TransactionId { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the reason for the reversal.
    /// </summary>
    [Required(ErrorMessage = "Reversal reason is required")]
    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;
}