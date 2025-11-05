using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
///     Request model for updating customer contact information based on CustomerUpdateContactDto.
/// </summary>
public sealed record UpdateCustomerContactRequest
{
    /// <summary>
    ///     Gets or sets the unique identifier of the organization associated with the customer.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the customer's mobile phone number.
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number format")]
    [JsonPropertyName("mobileNumber")]
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's email address.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;
}