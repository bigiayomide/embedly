using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for simulating wallet inflow (staging environment only).
/// </summary>
public sealed record SimulateInflowRequest
{
    /// <summary>
    ///     Gets or sets the destination account number.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the amount to credit.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Amount must be greater than 0")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }

    /// <summary>
    ///     Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; init; }

    /// <summary>
    ///     Gets or sets the narration/remarks for the transaction.
    /// </summary>
    [JsonPropertyName("narration")]
    public string? Narration { get; init; }

    /// <summary>
    ///     Gets or sets the sender's name.
    /// </summary>
    [JsonPropertyName("senderName")]
    public string? SenderName { get; init; }

    /// <summary>
    ///     Gets or sets the sender's account number.
    /// </summary>
    [JsonPropertyName("senderAccountNumber")]
    public string? SenderAccountNumber { get; init; }

    /// <summary>
    ///     Gets or sets the sender's bank name.
    /// </summary>
    [JsonPropertyName("senderBankName")]
    public string? SenderBankName { get; init; }
}
