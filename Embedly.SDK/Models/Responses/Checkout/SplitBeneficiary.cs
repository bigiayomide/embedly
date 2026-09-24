using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Checkout;

/// <summary>
///     Represents a split payment beneficiary.
///     <see cref="OrganizationId" />, <see cref="BankName" />, <see cref="CreatedAt" /> and
///     <see cref="UpdatedAt" /> are only populated by the split beneficiary endpoints, not when the
///     beneficiary is nested in a checkout wallet's split configurations.
/// </summary>
public sealed class SplitBeneficiary
{
    /// <summary>
    ///     Gets or sets the beneficiary ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid? OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the beneficiary's account name.
    /// </summary>
    [JsonPropertyName("beneficiaryName")]
    public string? BeneficiaryName { get; set; }

    /// <summary>
    ///     Gets or sets the beneficiary's account number.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the beneficiary's bank code.
    /// </summary>
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }

    /// <summary>
    ///     Gets or sets the beneficiary's bank name.
    /// </summary>
    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }

    /// <summary>
    ///     Gets or sets the beneficiary alias.
    /// </summary>
    [JsonPropertyName("beneficiaryAlias")]
    public string? BeneficiaryAlias { get; set; }

    /// <summary>
    ///     Gets or sets whether the beneficiary is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    ///     Gets or sets the date when the beneficiary was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the beneficiary was last updated.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
