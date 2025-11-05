using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
///     Represents a customer within an organization.
/// </summary>
public sealed class Customer
{
    /// <summary>
    ///     Gets or sets the unique identifier of the customer.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the unique identifier of the organization associated with the customer.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the first name of the customer.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the last name of the customer.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the middle name of the customer.
    /// </summary>
    [JsonPropertyName("middleName")]
    public string MiddleName { get; set; } = string.Empty;

    /// <summary>
    ///     ets or sets the date of birth of the customer, if available.
    /// </summary>
    [JsonPropertyName("dob")]
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier for the customer type.
    /// </summary>
    [JsonPropertyName("customerTypeId")]
    public Guid CustomerTypeId { get; set; }

    /// <summary>
    ///     Gets or sets the tier level of the customer.
    /// </summary>
    [JsonPropertyName("customerTierId")]
    public int CustomerTierId { get; set; }

    /// <summary>
    ///     Gets or sets an alternate or display name for the customer, if any.
    /// </summary>
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier of the customer’s country.
    /// </summary>
    [JsonPropertyName("countryId")]
    public Guid CountryId { get; set; }

    /// <summary>
    ///     Gets or sets the city where the customer resides.
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the residential or business address of the customer.
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's mobile phone number.
    /// </summary>
    [JsonPropertyName("mobileNumber")]
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the email address of the customer.
    /// </summary>
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the date when the customer record was created, if available.
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTimeOffset? DateCreated { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the corporate customer has been verified.
    /// </summary>
    [JsonPropertyName("isCorporateVerified")]
    public string IsCorporateVerified { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's verification status.
    /// </summary>
    [JsonPropertyName("verificationStatus")]
    public CustomerVerificationStatus VerificationStatus { get; set; }

    /// <summary>
    ///     Gets or sets the customer type.
    /// </summary>
    [JsonPropertyName("customerType")]
    public CustomerType? CustomerType { get; set; }

    /// <summary>
    ///     Gets or sets the customer status.
    /// </summary>
    [JsonPropertyName("status")]
    public CustomerStatus Status { get; set; }

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