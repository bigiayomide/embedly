using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
/// Request model for upgrading customer KYC using NIN (National Identification Number).
/// </summary>
public sealed class NinKycUpgradeRequest
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the National Identification Number (NIN).
    /// </summary>
    [Required(ErrorMessage = "NIN is required")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "NIN must be 11 digits")]
    [JsonPropertyName("nin")]
    public string Nin { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer's date of birth for verification.
    /// </summary>
    [Required(ErrorMessage = "Date of birth is required")]
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }
}