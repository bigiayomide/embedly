using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
///     Represents a customer in the Embedly system.
/// </summary>
public sealed class Customer
{
    /// <summary>
    ///     Gets or sets the unique customer identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's first name.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's last name.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's full name.
    /// </summary>
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }

    /// <summary>
    ///     Gets or sets the customer's email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's phone number.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's date of birth.
    /// </summary>
    [JsonPropertyName("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets the customer's gender.
    /// </summary>
    [JsonPropertyName("gender")]
    public string? Gender { get; set; }

    /// <summary>
    ///     Gets or sets the customer's verification status.
    /// </summary>
    [JsonPropertyName("verificationStatus")]
    public CustomerVerificationStatus VerificationStatus { get; set; }

    /// <summary>
    ///     Gets or sets the customer's KYC level.
    /// </summary>
    [JsonPropertyName("kycLevel")]
    public string? KycLevel { get; set; }

    /// <summary>
    ///     Gets or sets the customer type.
    /// </summary>
    [JsonPropertyName("customerType")]
    public CustomerType? CustomerType { get; set; }

    /// <summary>
    ///     Gets or sets the customer's address.
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    /// <summary>
    ///     Gets or sets the customer status.
    /// </summary>
    [JsonPropertyName("status")]
    public CustomerStatus Status { get; set; }

    /// <summary>
    ///     Gets or sets the date when the customer was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the customer was last updated.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    ///     Gets or sets additional metadata for the customer.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object?>? Metadata { get; set; }
}

/// <summary>
///     Customer verification status.
/// </summary>
public enum CustomerVerificationStatus
{
    /// <summary>
    ///     Customer is not verified.
    /// </summary>
    [JsonPropertyName("unverified")] Unverified,

    /// <summary>
    ///     Customer verification is pending.
    /// </summary>
    [JsonPropertyName("pending")] Pending,

    /// <summary>
    ///     Customer is verified.
    /// </summary>
    [JsonPropertyName("verified")] Verified,

    /// <summary>
    ///     Customer verification was rejected.
    /// </summary>
    [JsonPropertyName("rejected")] Rejected
}

/// <summary>
///     Customer status.
/// </summary>
public enum CustomerStatus
{
    /// <summary>
    ///     Customer account is active.
    /// </summary>
    [JsonPropertyName("active")] Active,

    /// <summary>
    ///     Customer account is inactive.
    /// </summary>
    [JsonPropertyName("inactive")] Inactive,

    /// <summary>
    ///     Customer account is suspended.
    /// </summary>
    [JsonPropertyName("suspended")] Suspended,

    /// <summary>
    ///     Customer account is closed.
    /// </summary>
    [JsonPropertyName("closed")] Closed
}

/// <summary>
///     Customer type information.
/// </summary>
public sealed class CustomerType
{
    /// <summary>
    ///     Gets or sets the customer type identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer type name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer type description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

/// <summary>
///     Customer address information.
/// </summary>
public sealed class Address
{
    /// <summary>
    ///     Gets or sets the street address.
    /// </summary>
    [JsonPropertyName("street")]
    public string? Street { get; set; }

    /// <summary>
    ///     Gets or sets the city.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>
    ///     Gets or sets the state or province.
    /// </summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }

    /// <summary>
    ///     Gets or sets the postal code.
    /// </summary>
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    /// <summary>
    ///     Gets or sets the country code.
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; } = "NG";

    /// <summary>
    ///     Gets or sets the full formatted address.
    /// </summary>
    [JsonPropertyName("formattedAddress")]
    public string? FormattedAddress { get; set; }
}