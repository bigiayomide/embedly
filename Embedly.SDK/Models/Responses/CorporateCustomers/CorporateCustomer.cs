using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Requests.CorporateCustomers;

namespace Embedly.SDK.Models.Responses.CorporateCustomers;

/// <summary>
///     Represents a corporate customer in the Embedly system.
/// </summary>
public sealed class CorporateCustomer
{
    /// <summary>
    ///     Gets or sets the unique corporate customer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the business registration number (RC Number).
    /// </summary>
    [JsonPropertyName("rcNumber")]
    public string RcNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the Tax Identification Number (TIN).
    /// </summary>
    [JsonPropertyName("tin")]
    public string Tin { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the full business name.
    /// </summary>
    [JsonPropertyName("fullBusinessName")]
    public string FullBusinessName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the business address.
    /// </summary>
    [JsonPropertyName("businessAddress")]
    public string BusinessAddress { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country ID.
    /// </summary>
    [JsonPropertyName("countryId")]
    public Guid CountryId { get; set; }

    /// <summary>
    ///     Gets or sets the business city.
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the business email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the wallet preferred name.
    /// </summary>
    [JsonPropertyName("walletPreferredName")]
    public string WalletPreferredName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the list of directors.
    /// </summary>
    [JsonPropertyName("directors")]
    public List<Director>? Directors { get; set; }

    /// <summary>
    ///     Gets or sets the list of documents.
    /// </summary>
    [JsonPropertyName("documents")]
    public List<CorporateDocument>? Documents { get; set; }

    /// <summary>
    ///     Gets or sets the date when the corporate customer was created.
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the corporate customer was last updated.
    /// </summary>
    [JsonPropertyName("dateModified")]
    public DateTimeOffset? UpdatedAt { get; set; }
    
    /// <summary>
    ///     Gets corperate customer account verification status.
    /// </summary>
    [JsonPropertyName("isCorporateVerified")]
    public string? IsCorporateVerified { get; set; }
}

/// <summary>
///     Corporate customer verification status.
/// </summary>
public enum CorporateVerificationStatus
{
    /// <summary>
    ///     Corporate customer is not verified.
    /// </summary>
    [JsonPropertyName("unverified")]
    Unverified,

    /// <summary>
    ///     Corporate customer verification is pending.
    /// </summary>
    [JsonPropertyName("pending")]
    Pending,

    /// <summary>
    ///     Corporate customer is verified.
    /// </summary>
    [JsonPropertyName("verified")]
    Verified,

    /// <summary>
    ///     Corporate customer verification was rejected.
    /// </summary>
    [JsonPropertyName("rejected")]
    Rejected,
    
    /// <summary>
    ///     Corporate customer verification was Approved.
    /// </summary>
    [JsonPropertyName("approved")]
    Approved
}

/// <summary>
///     Corporate customer status.
/// </summary>
public enum CorporateCustomerStatus
{
    /// <summary>
    ///     Corporate customer account is active.
    /// </summary>
    [JsonPropertyName("active")]
    Active,

    /// <summary>
    ///     Corporate customer account is inactive.
    /// </summary>
    [JsonPropertyName("inactive")]
    Inactive,

    /// <summary>
    ///     Corporate customer account is suspended.
    /// </summary>
    [JsonPropertyName("suspended")]
    Suspended,

    /// <summary>
    ///     Corporate customer account is closed.
    /// </summary>
    [JsonPropertyName("closed")]
    Closed
}