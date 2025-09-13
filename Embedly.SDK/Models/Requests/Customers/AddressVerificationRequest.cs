using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
/// Request model for customer address verification.
/// </summary>
public sealed class AddressVerificationRequest
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the street address.
    /// </summary>
    [Required(ErrorMessage = "Street address is required")]
    [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
    [JsonPropertyName("street")]
    public string Street { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    [Required(ErrorMessage = "City is required")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    [Required(ErrorMessage = "State is required")]
    [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the postal code.
    /// </summary>
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }
    
    /// <summary>
    /// Gets or sets the country code.
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; } = "NG";
    
    /// <summary>
    /// Gets or sets the verification method (e.g., "utility_bill", "bank_statement").
    /// </summary>
    [Required(ErrorMessage = "Verification method is required")]
    [JsonPropertyName("verificationMethod")]
    public string VerificationMethod { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the verification document or evidence.
    /// </summary>
    [JsonPropertyName("verificationDocument")]
    public string? VerificationDocument { get; set; }
}