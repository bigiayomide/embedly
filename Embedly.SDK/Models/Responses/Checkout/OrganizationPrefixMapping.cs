using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Checkout;

/// <summary>
///     Represents an organization prefix mapping for checkout wallets.
/// </summary>
public sealed class OrganizationPrefixMapping
{
    /// <summary>
    ///     Gets or sets the mapping ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the secondary prefix.
    /// </summary>
    [JsonPropertyName("secondaryPrefix")]
    public string? SecondaryPrefix { get; set; }

    /// <summary>
    ///     Gets or sets the primary prefix ID.
    /// </summary>
    [JsonPropertyName("primaryPrefixId")]
    public Guid PrimaryPrefixId { get; set; }

    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the mapping alias.
    /// </summary>
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    /// <summary>
    ///     Gets or sets the organization name.
    /// </summary>
    [JsonPropertyName("organizationName")]
    public string? OrganizationName { get; set; }

    /// <summary>
    ///     Gets or sets the organization's active status (e.g. "active").
    /// </summary>
    [JsonPropertyName("organizationIsActive")]
    public string? OrganizationIsActive { get; set; }

    /// <summary>
    ///     Gets or sets the primary prefix code (e.g. "981").
    /// </summary>
    [JsonPropertyName("primaryPrefixCode")]
    public string? PrimaryPrefixCode { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the wallet that checkout payments are settled into.
    /// </summary>
    [JsonPropertyName("settlementWalletId")]
    public Guid? SettlementWalletId { get; set; }

    /// <summary>
    ///     Gets or sets the account number of the settlement wallet.
    /// </summary>
    [JsonPropertyName("settlementAccountNumber")]
    public string? SettlementAccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the checkout type ID, if a checkout type is assigned.
    /// </summary>
    [JsonPropertyName("checkoutTypeId")]
    public Guid? CheckoutTypeId { get; set; }

    /// <summary>
    ///     Gets or sets the checkout type code (e.g. "GENERIC"), if a checkout type is assigned.
    /// </summary>
    [JsonPropertyName("checkoutTypeCode")]
    public string? CheckoutTypeCode { get; set; }

    /// <summary>
    ///     Gets or sets the checkout type name (e.g. "Generic Checkout Type"), if a checkout type is assigned.
    /// </summary>
    [JsonPropertyName("checkoutTypeName")]
    public string? CheckoutTypeName { get; set; }

    /// <summary>
    ///     Gets or sets the disbursement frequency, if configured.
    /// </summary>
    [JsonPropertyName("disbursementFrequency")]
    public string? DisbursementFrequency { get; set; }

    /// <summary>
    ///     Gets or sets whether payments that don't match the expected amount are automatically reversed.
    /// </summary>
    [JsonPropertyName("autoReversalForAmountMismatch")]
    public bool AutoReversalForAmountMismatch { get; set; }

    /// <summary>
    ///     Gets or sets the date when the mapping was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}
