using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
///     Request model for creating a new customer based on Embedly API CustomerDto.
/// </summary>
public sealed class CreateCustomerRequest
{
    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [Required(ErrorMessage = "Organization ID is required")]
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the customer's first name.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    /// <summary>
    ///     Gets or sets the customer's last name.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    /// <summary>
    ///     Gets or sets the customer's middle name.
    /// </summary>
    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    /// <summary>
    ///     Gets or sets the customer's date of birth.
    /// </summary>
    [JsonPropertyName("dob")]
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets the customer type ID.
    /// </summary>
    [Required(ErrorMessage = "Customer type ID is required")]
    [JsonPropertyName("customerTypeId")]
    public Guid CustomerTypeId { get; set; }

    /// <summary>
    ///     Gets or sets the customer alias.
    /// </summary>
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    /// <summary>
    ///     Gets or sets the country ID.
    /// </summary>
    [Required(ErrorMessage = "Country ID is required")]
    [JsonPropertyName("countryId")]
    public Guid CountryId { get; set; }

    /// <summary>
    ///     Gets or sets the customer's city.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>
    ///     Gets or sets the customer's address.
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    /// <summary>
    ///     Gets or sets the customer's mobile number.
    /// </summary>
    [JsonPropertyName("mobileNumber")]
    public string? MobileNumber { get; set; }

    /// <summary>
    ///     Gets or sets the customer's email address.
    /// </summary>
    [JsonPropertyName("emailAddress")]
    public string? EmailAddress { get; set; }
}