using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Checkout;

/// <summary>
///     Represents a single checkout generated on a checkout wallet.
/// </summary>
public sealed class CheckoutWalletHistory
{
    /// <summary>
    ///     Gets or sets the history entry ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the checkout reference (e.g. "CHK202510290916306114570").
    /// </summary>
    [JsonPropertyName("checkoutRef")]
    public string? CheckoutRef { get; set; }

    /// <summary>
    ///     Gets or sets the expected amount for this checkout.
    /// </summary>
    [JsonPropertyName("expectedAmount")]
    public decimal ExpectedAmount { get; set; }

    /// <summary>
    ///     Gets or sets the date when the checkout was generated.
    /// </summary>
    [JsonPropertyName("generatedAt")]
    public DateTime GeneratedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the checkout was used.
    /// </summary>
    [JsonPropertyName("usedAt")]
    public DateTime? UsedAt { get; set; }

    /// <summary>
    ///     Gets or sets the checkout status (e.g. "Used").
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the transaction that settled this checkout, if any.
    /// </summary>
    [JsonPropertyName("transactionId")]
    public Guid? TransactionId { get; set; }
}
