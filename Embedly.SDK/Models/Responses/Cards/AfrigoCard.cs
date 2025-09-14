using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Cards;

/// <summary>
///     Represents an Afrigo card in the Embedly system.
/// </summary>
public sealed record AfrigoCard
{
    /// <summary>
    ///     Gets or sets the unique card identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the card number (masked).
    /// </summary>
    [JsonPropertyName("cardNumber")]
    public string CardNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the card type.
    /// </summary>
    [JsonPropertyName("cardType")]
    public string CardType { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the card status.
    /// </summary>
    [JsonPropertyName("status")]
    public CardStatus Status { get; init; }

    /// <summary>
    ///     Gets or sets the customer ID who owns the card.
    /// </summary>
    [JsonPropertyName("customerId")]
    public string CustomerId { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the card expiry date.
    /// </summary>
    [JsonPropertyName("expiryDate")]
    public string? ExpiryDate { get; init; }

    /// <summary>
    ///     Gets or sets the card CVV (for virtual cards).
    /// </summary>
    [JsonPropertyName("cvv")]
    public string? Cvv { get; init; }

    /// <summary>
    ///     Gets or sets the cardholder's name.
    /// </summary>
    [JsonPropertyName("cardholderName")]
    public string? CardholderName { get; init; }

    /// <summary>
    ///     Gets or sets the card spending limit.
    /// </summary>
    [JsonPropertyName("spendingLimit")]
    public decimal SpendingLimit { get; set; }

    /// <summary>
    ///     Gets or sets the available balance on the card.
    /// </summary>
    [JsonPropertyName("availableBalance")]
    public decimal AvailableBalance { get; set; }

    /// <summary>
    ///     Gets or sets the currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = "NGN";

    /// <summary>
    ///     Gets or sets the date when the card was issued.
    /// </summary>
    [JsonPropertyName("issuedAt")]
    public DateTimeOffset IssuedAt { get; init; }

    /// <summary>
    ///     Gets or sets the date when the card was activated.
    /// </summary>
    [JsonPropertyName("activatedAt")]
    public DateTimeOffset? ActivatedAt { get; init; }

    /// <summary>
    ///     Gets or sets the date when the card was last updated.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset? UpdatedAt { get; set; }
}

/// <summary>
///     Card status enumeration.
/// </summary>
public enum CardStatus
{
    /// <summary>
    ///     Card is issued but not yet activated.
    /// </summary>
    [JsonPropertyName("issued")] Issued,

    /// <summary>
    ///     Card is active and can be used.
    /// </summary>
    [JsonPropertyName("active")] Active,

    /// <summary>
    ///     Card is temporarily blocked.
    /// </summary>
    [JsonPropertyName("blocked")] Blocked,

    /// <summary>
    ///     Card is expired.
    /// </summary>
    [JsonPropertyName("expired")] Expired,

    /// <summary>
    ///     Card is cancelled.
    /// </summary>
    [JsonPropertyName("cancelled")] Cancelled
}