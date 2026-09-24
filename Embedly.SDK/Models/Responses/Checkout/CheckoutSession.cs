using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Checkout;

/// <summary>
///     Represents checkout wallet details based on WalletDetails schema.
/// </summary>
public sealed class CheckoutWallet
{
    /// <summary>
    ///     Gets or sets the wallet ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the checkout reference (e.g. "CHK202507241358013544205").
    /// </summary>
    [JsonPropertyName("checkoutRef")]
    public string? CheckoutRef { get; set; }

    /// <summary>
    ///     Gets or sets the wallet name.
    /// </summary>
    [JsonPropertyName("walletName")]
    public string? WalletName { get; set; }

    /// <summary>
    ///     Gets or sets the wallet number.
    /// </summary>
    [JsonPropertyName("walletNumber")]
    public string? WalletNumber { get; set; }

    /// <summary>
    ///     Gets or sets the expected amount.
    /// </summary>
    [JsonPropertyName("expectedAmount")]
    public decimal ExpectedAmount { get; set; }

    /// <summary>
    ///     Gets or sets the wallet status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    ///     Gets or sets the date when the wallet was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the wallet expires.
    /// </summary>
    [JsonPropertyName("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the wallet was used.
    /// </summary>
    [JsonPropertyName("usedAt")]
    public DateTime? UsedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the wallet expired.
    /// </summary>
    [JsonPropertyName("expiredAt")]
    public DateTime? ExpiredAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the wallet was reactivated.
    /// </summary>
    [JsonPropertyName("reactivatedAt")]
    public DateTime? ReactivatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the invoice reference.
    /// </summary>
    [JsonPropertyName("invoiceReference")]
    public string? InvoiceReference { get; set; }

    /// <summary>
    ///     Gets or sets the wallet description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets the currency code (e.g. "NGN").
    /// </summary>
    [JsonPropertyName("currencyCode")]
    public string? CurrencyCode { get; set; }

    /// <summary>
    ///     Gets or sets the customer's email address.
    /// </summary>
    [JsonPropertyName("customerEmail")]
    public string? CustomerEmail { get; set; }

    /// <summary>
    ///     Gets or sets the customer's name.
    /// </summary>
    [JsonPropertyName("customerName")]
    public string? CustomerName { get; set; }

    /// <summary>
    ///     Gets or sets additional metadata, as supplied when the wallet was created (typically a JSON string).
    /// </summary>
    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }

    /// <summary>
    ///     Gets or sets the split type ("Fixed" or "Percentage"), if this is a split payment.
    /// </summary>
    [JsonPropertyName("splitType")]
    public string? SplitType { get; set; }

    /// <summary>
    ///     Gets or sets how payments to this wallet are split across beneficiaries.
    /// </summary>
    [JsonPropertyName("splitConfigurations")]
    public List<CheckoutSplitConfiguration>? SplitConfigurations { get; set; }

    /// <summary>
    ///     Gets or sets the wallet's checkout history (one entry per generated checkout).
    /// </summary>
    [JsonPropertyName("walletHistories")]
    public List<CheckoutWalletHistory>? WalletHistories { get; set; }

    /// <summary>
    ///     Gets or sets the transactions received by the wallet.
    ///     Populated by <c>GetCheckoutWalletWithTransactionsAsync</c>.
    /// </summary>
    [JsonPropertyName("transactions")]
    public List<CheckoutTransaction>? Transactions { get; set; }

    /// <summary>
    ///     Checks if the checkout wallet has expired.
    /// </summary>
    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }

    /// <summary>
    ///     Checks if the checkout wallet has been used.
    /// </summary>
    public bool IsUsed()
    {
        return UsedAt.HasValue;
    }
}