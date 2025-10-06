using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.CorporateCustomers;

/// <summary>
///     Request model for adding a director to a corporate customer.
/// </summary>
public sealed class AddDirectorRequest
{
    /// <summary>
    ///     Gets or sets the director's first name.
    /// </summary>
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's last name.
    /// </summary>
    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's middle name.
    /// </summary>
    [MaxLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    /// <summary>
    ///     Gets or sets the director's email address.
    /// </summary>
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's phone number.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's date of birth.
    /// </summary>
    [Required(ErrorMessage = "Date of birth is required")]
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets the director's residential address.
    /// </summary>
    [Required(ErrorMessage = "Address is required")]
    [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's BVN (Bank Verification Number).
    /// </summary>
    [Required(ErrorMessage = "BVN is required")]
    [MaxLength(11, ErrorMessage = "BVN cannot exceed 11 characters")]
    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's NIN (National Identification Number).
    /// </summary>
    [Required(ErrorMessage = "NIN is required")]
    [MaxLength(11, ErrorMessage = "NIN cannot exceed 11 characters")]
    [JsonPropertyName("nin")]
    public string Nin { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the director's meter number.
    /// </summary>
    [Required(ErrorMessage = "Meter number is required")]
    [MaxLength(50, ErrorMessage = "Meter number cannot exceed 50 characters")]
    [JsonPropertyName("meterNumber")]
    public string MeterNumber { get; set; } = string.Empty;
}