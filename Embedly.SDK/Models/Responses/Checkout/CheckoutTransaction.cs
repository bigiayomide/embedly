using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Checkout;

/// <summary>
///     Represents a payment received by a checkout wallet.
/// </summary>
public sealed class CheckoutTransaction
{
    /// <summary>
    ///     Gets or sets the transaction ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the transaction amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    ///     Gets or sets the sender's account number.
    /// </summary>
    [JsonPropertyName("senderAccountNumber")]
    public string? SenderAccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the sender's name.
    /// </summary>
    [JsonPropertyName("senderName")]
    public string? SenderName { get; set; }

    /// <summary>
    ///     Gets or sets the recipient (checkout wallet) account number.
    /// </summary>
    [JsonPropertyName("recipientAccountNumber")]
    public string? RecipientAccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the recipient (checkout wallet) name.
    /// </summary>
    [JsonPropertyName("recipientName")]
    public string? RecipientName { get; set; }

    /// <summary>
    ///     Gets or sets the organization settlement account the funds were settled to.
    /// </summary>
    [JsonPropertyName("organizationSettlementAccount")]
    public string? OrganizationSettlementAccount { get; set; }

    /// <summary>
    ///     Gets or sets the transaction status (e.g. "Completed").
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    ///     Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    /// <summary>
    ///     Gets or sets the date when the transaction was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the transaction was completed.
    /// </summary>
    [JsonPropertyName("completedAt")]
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    ///     Gets or sets the NIP session ID.
    /// </summary>
    [JsonPropertyName("sessionId")]
    public string? SessionId { get; set; }

    /// <summary>
    ///     Gets or sets the reversal ID, if the transaction was reversed.
    /// </summary>
    [JsonPropertyName("reversalId")]
    public string? ReversalId { get; set; }

    /// <summary>
    ///     Gets or sets the date when a reversal was attempted.
    /// </summary>
    [JsonPropertyName("reversalAttemptedAt")]
    public DateTime? ReversalAttemptedAt { get; set; }
}
