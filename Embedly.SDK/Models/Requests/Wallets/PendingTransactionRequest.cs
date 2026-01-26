using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for posting a pending wallet transaction.
/// </summary>
public sealed record PendingTransactionRequest
{
    /// <summary>
    ///     Gets or sets the transaction reference.
    /// </summary>
    [Required(ErrorMessage = "Transaction reference is required")]
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the source account number.
    /// </summary>
    [JsonPropertyName("sourceAccountNumber")]
    public string? SourceAccountNumber { get; init; }

    /// <summary>
    ///     Gets or sets the destination account number.
    /// </summary>
    [JsonPropertyName("destinationAccountNumber")]
    public string? DestinationAccountNumber { get; init; }

    /// <summary>
    ///     Gets or sets the transaction amount.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Amount must be greater than 0")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }

    /// <summary>
    ///     Gets or sets the transaction description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    ///     Gets or sets the transaction type.
    /// </summary>
    [JsonPropertyName("transactionType")]
    public string? TransactionType { get; init; }
}