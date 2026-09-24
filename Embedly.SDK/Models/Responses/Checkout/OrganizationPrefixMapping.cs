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
}
