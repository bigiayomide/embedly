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
    ///     Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the primary prefix.
    /// </summary>
    [JsonPropertyName("primaryPrefix")]
    public string? PrimaryPrefix { get; set; }

    /// <summary>
    ///     Gets or sets the secondary prefix.
    /// </summary>
    [JsonPropertyName("secondaryPrefix")]
    public string? SecondaryPrefix { get; set; }

    /// <summary>
    ///     Gets or sets the bank name.
    /// </summary>
    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }

    /// <summary>
    ///     Gets or sets the bank code.
    /// </summary>
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }

    /// <summary>
    ///     Gets or sets whether this mapping is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    ///     Gets or sets the creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the last updated date.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
