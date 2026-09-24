using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Requests.CorporateCustomers;

namespace Embedly.SDK.Models.Requests.Checkout;

/// <summary>
///     Request model for creating a split payment beneficiary for checkout wallets.
/// </summary>
public sealed record CreateSplitBeneficiaryRequest
{
    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [NonEmptyGuid(ErrorMessage = "Organization ID is required")]
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }

    /// <summary>
    ///     Gets or sets the beneficiary's account name.
    /// </summary>
    [Required(ErrorMessage = "Beneficiary name is required")]
    [JsonPropertyName("beneficiaryName")]
    public string BeneficiaryName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the beneficiary's account number.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the beneficiary's bank code.
    /// </summary>
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; init; }

    /// <summary>
    ///     Gets or sets the beneficiary's bank name.
    /// </summary>
    [JsonPropertyName("bankName")]
    public string? BankName { get; init; }

    /// <summary>
    ///     Gets or sets an alias for the beneficiary.
    /// </summary>
    [JsonPropertyName("beneficiaryAlias")]
    public string? BeneficiaryAlias { get; init; }
}
