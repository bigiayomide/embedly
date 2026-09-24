using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Requests.CorporateCustomers;

namespace Embedly.SDK.Models.Requests.Checkout;

/// <summary>
///     Request model for generating checkout wallet based on GenerateCheckoutWalletCommand schema.
/// </summary>
public sealed record GenerateCheckoutWalletRequest
{
    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [NonEmptyGuid(ErrorMessage = "Organization ID is required")]
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }

    /// <summary>
    ///     Gets or sets the expected amount.
    /// </summary>
    [Required(ErrorMessage = "Expected amount is required")]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Expected amount must be greater than 0")]
    [JsonPropertyName("expectedAmount")]
    public decimal ExpectedAmount { get; init; }

    /// <summary>
    ///     Gets or sets the organization prefix mapping ID.
    /// </summary>
    [Required(ErrorMessage = "Organization prefix mapping ID is required")]
    [JsonPropertyName("organizationPrefixMappingId")]
    public Guid OrganizationPrefixMappingId { get; init; }

    /// <summary>
    ///     Gets or sets the expiry duration in minutes. Defaults to 30 minutes if not specified.
    /// </summary>
    [JsonPropertyName("expiryDurationMinutes")]
    public int? ExpiryDurationMinutes { get; init; } = 30;

    /// <summary>
    ///     Gets or sets the invoice reference.
    /// </summary>
    [JsonPropertyName("invoiceReference")]
    public string? InvoiceReference { get; init; }

    /// <summary>
    ///     Gets or sets the wallet description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    ///     Gets or sets the currency code (e.g. "NGN").
    /// </summary>
    [JsonPropertyName("currencyCode")]
    public string? CurrencyCode { get; init; }

    /// <summary>
    ///     Gets or sets the customer's email address.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid customer email address")]
    [JsonPropertyName("customerEmail")]
    public string? CustomerEmail { get; init; }

    /// <summary>
    ///     Gets or sets the customer's name.
    /// </summary>
    [JsonPropertyName("customerName")]
    public string? CustomerName { get; init; }

    /// <summary>
    ///     Gets or sets additional metadata.
    /// </summary>
    [JsonPropertyName("metadata")]
    public string? Metadata { get; init; }

    /// <summary>
    ///     Gets or sets the split type. Valid values: "Fixed", "Percentage". Required for split payments.
    /// </summary>
    [JsonPropertyName("splitType")]
    public string? SplitType { get; init; }

    /// <summary>
    ///     Gets or sets how the payment is distributed across split beneficiaries.
    /// </summary>
    [JsonPropertyName("incomeSplitConfig")]
    public List<IncomeSplitConfig>? IncomeSplitConfig { get; init; }

    /// <summary>
    ///     Gets or sets the name displayed during account validation (name enquiry).
    /// </summary>
    [JsonPropertyName("accountInquiryResponseName")]
    public string? AccountInquiryResponseName { get; init; }
}

/// <summary>
///     Split payment allocation for a single beneficiary of a checkout wallet.
/// </summary>
public sealed record IncomeSplitConfig
{
    /// <summary>
    ///     Gets or sets the beneficiary ID.
    /// </summary>
    [Required(ErrorMessage = "Beneficiary ID is required")]
    [JsonPropertyName("beneficiaryId")]
    public Guid BeneficiaryId { get; init; }

    /// <summary>
    ///     Gets or sets the amount or percentage allocated to the beneficiary, depending on the split type.
    /// </summary>
    [Required(ErrorMessage = "Split value is required")]
    [JsonPropertyName("splitValue")]
    public decimal SplitValue { get; init; }

    /// <summary>
    ///     Gets or sets the fee amount to be deducted.
    /// </summary>
    [Required(ErrorMessage = "Fee value is required")]
    [JsonPropertyName("feeValue")]
    public decimal FeeValue { get; init; }

    /// <summary>
    ///     Gets or sets whether this beneficiary bears the transaction fees.
    /// </summary>
    [Required(ErrorMessage = "Fee bearer is required")]
    [JsonPropertyName("feeBearer")]
    public bool FeeBearer { get; init; }
}