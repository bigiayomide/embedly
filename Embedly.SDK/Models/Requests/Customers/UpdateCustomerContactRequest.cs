using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
/// Request model for updating customer contact information based on CustomerUpdateContactDto.
/// </summary>
public sealed record UpdateCustomerContactRequest
{
    /// <summary>
    /// Gets or sets the customer's email address.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [JsonPropertyName("email")]
    public string? Email { get; init; }
    
    /// <summary>
    /// Gets or sets the customer's phone number.
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number format")]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; init; }
    
    /// <summary>
    /// Gets or sets the customer's address.
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; init; }
}