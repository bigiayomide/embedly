using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.CorporateCustomers;

/// <summary>
///     Represents a director of a corporate customer.
/// </summary>
public sealed class Director
{
    /// <summary>
    ///     Gets or sets the unique director identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the director's first name.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's last name.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's middle name.
    /// </summary>
    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    /// <summary>
    ///     Gets or sets the director's email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's phone number.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's date of birth.
    /// </summary>
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets the director's residential address.
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's BVN (Bank Verification Number).
    /// </summary>
    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's NIN (National Identification Number).
    /// </summary>
    [JsonPropertyName("nin")]
    public string Nin { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's meter number.
    /// </summary>
    [JsonPropertyName("meterNumber")]
    public string MeterNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the date when the director was added.
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the director was last updated.
    /// </summary>
    [JsonPropertyName("dateModified")]
    public DateTimeOffset? UpdatedAt { get; set; }
}

/// <summary>
///     Director verification status.
/// </summary>
public enum DirectorVerificationStatus
{
    /// <summary>
    ///     Director is not verified.
    /// </summary>
    [JsonPropertyName("unverified")]
    Unverified,

    /// <summary>
    ///     Director verification is pending.
    /// </summary>
    [JsonPropertyName("pending")]
    Pending,

    /// <summary>
    ///     Director is verified.
    /// </summary>
    [JsonPropertyName("verified")]
    Verified,

    /// <summary>
    ///     Director verification was rejected.
    /// </summary>
    [JsonPropertyName("rejected")]
    Rejected
}

/// <summary>
///     Director status.
/// </summary>
public enum DirectorStatus
{
    /// <summary>
    ///     Director is active.
    /// </summary>
    [JsonPropertyName("active")]
    Active,

    /// <summary>
    ///     Director is inactive.
    /// </summary>
    [JsonPropertyName("inactive")]
    Inactive,

    /// <summary>
    ///     Director has resigned.
    /// </summary>
    [JsonPropertyName("resigned")]
    Resigned,

    /// <summary>
    ///     Director was removed.
    /// </summary>
    [JsonPropertyName("removed")]
    Removed
}