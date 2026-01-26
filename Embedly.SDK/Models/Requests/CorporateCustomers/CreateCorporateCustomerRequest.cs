using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.CorporateCustomers;

/// <summary>
///     Validation attribute that ensures a Guid is not empty.
/// </summary>
public sealed class NonEmptyGuidAttribute : ValidationAttribute
{
    /// <summary>
    ///     Determines whether the specified value is a non-empty Guid.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>true if the value is a non-empty Guid; otherwise, false.</returns>
    public override bool IsValid(object? value)
    {
        if (value is null)
            return true; // Let [Required] handle null

        if (value is Guid guid)
            return guid != Guid.Empty;

        return false;
    }
}

/// <summary>
///     Request model for creating a new corporate customer.
/// </summary>
public sealed class CreateCorporateCustomerRequest
{
    /// <summary>
    ///     Gets or sets the business registration number (RC Number).
    /// </summary>
    [Required(ErrorMessage = "RC Number is required")]
    [MaxLength(15, ErrorMessage = "RC Number cannot exceed 15 characters")]
    [JsonPropertyName("rcNumber")]
    public string RcNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the Tax Identification Number (TIN).
    /// </summary>
    [Required(ErrorMessage = "TIN is required")]
    [MaxLength(15, ErrorMessage = "TIN cannot exceed 15 characters")]
    [JsonPropertyName("tin")]
    public string Tin { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the full business name.
    /// </summary>
    [Required(ErrorMessage = "Full business name is required")]
    [MaxLength(150, ErrorMessage = "Full business name cannot exceed 150 characters")]
    [JsonPropertyName("fullBusinessName")]
    public string FullBusinessName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the business address.
    /// </summary>
    [Required(ErrorMessage = "Business address is required")]
    [MaxLength(250, ErrorMessage = "Business address cannot exceed 250 characters")]
    [JsonPropertyName("businessAddress")]
    public string BusinessAddress { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country ID.
    /// </summary>
    [NonEmptyGuid(ErrorMessage = "Country ID is required")]
    [JsonPropertyName("countryId")]
    public Guid CountryId { get; set; }

    /// <summary>
    ///     Gets or sets the business city.
    /// </summary>
    [Required(ErrorMessage = "City is required")]
    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the business email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the wallet preferred name.
    /// </summary>
    [Required(ErrorMessage = "Wallet preferred name is required")]
    [MaxLength(100, ErrorMessage = "Wallet preferred name cannot exceed 100 characters")]
    [JsonPropertyName("walletPreferredName")]
    public string WalletPreferredName { get; set; } = string.Empty;
}
