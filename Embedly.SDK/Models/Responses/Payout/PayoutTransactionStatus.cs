using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
///     Represents the response details for a payout transaction.
/// </summary>
public sealed class PayoutTransactionStatus
{
    /// <summary>
    /// Gets or sets the overall status of the transaction.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique reference assigned to the transaction.
    /// </summary>
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique reference provided by the payment provider.
    /// </summary>
    [JsonPropertyName("providerReference")]
    public string ProviderReference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reference associated with the payment.
    /// </summary>
    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the session identifier for the transaction process.
    /// </summary>
    [JsonPropertyName("sessionId")]
    public string SessionId { get; set; } = string.Empty;
}
