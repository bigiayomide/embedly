using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Checkout;

/// <summary>
///     Request model for generating checkout wallet based on GenerateCheckoutWalletCommand schema.
/// </summary>
public sealed record GenerateCheckoutWalletRequest
{
    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [Required(ErrorMessage = "Organization ID is required")]
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }

    /// <summary>
    ///     Gets or sets the expected amount.
    /// </summary>
    [Required(ErrorMessage = "Expected amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Expected amount must be greater than 0")]
    [JsonPropertyName("expectedAmount")]
    public double ExpectedAmount { get; init; }

    /// <summary>
    ///     Gets or sets the organization prefix mapping ID.
    /// </summary>
    [Required(ErrorMessage = "Organization prefix mapping ID is required")]
    [JsonPropertyName("organizationPrefixMappingId")]
    public Guid OrganizationPrefixMappingId { get; init; }
}