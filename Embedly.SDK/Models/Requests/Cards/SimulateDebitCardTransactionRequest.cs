using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Cards;

/// <summary>
///     Request model for simulating a debit card transaction (staging environment only).
/// </summary>
public sealed record SimulateDebitCardTransactionRequest
{
    /// <summary>
    ///     Gets or sets the account number.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [JsonPropertyName("AccountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the transaction amount.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [JsonPropertyName("amount")]
    public string Amount { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the transaction type. Valid values: "POS", "ATM".
    /// </summary>
    [Required(ErrorMessage = "Transaction type is required")]
    [JsonPropertyName("TransactionType")]
    public string TransactionType { get; init; } = string.Empty;
}
