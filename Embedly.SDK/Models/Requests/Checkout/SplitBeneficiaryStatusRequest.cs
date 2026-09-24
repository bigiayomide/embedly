using System;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Requests.CorporateCustomers;

namespace Embedly.SDK.Models.Requests.Checkout;

/// <summary>
///     Request model for activating or deactivating a split beneficiary.
/// </summary>
public sealed record SplitBeneficiaryStatusRequest
{
    /// <summary>
    ///     Gets or sets the ID of the beneficiary to activate or deactivate.
    /// </summary>
    [NonEmptyGuid(ErrorMessage = "Beneficiary ID is required")]
    [JsonPropertyName("beneficiaryId")]
    public Guid BeneficiaryId { get; init; }

    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [NonEmptyGuid(ErrorMessage = "Organization ID is required")]
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }
}
