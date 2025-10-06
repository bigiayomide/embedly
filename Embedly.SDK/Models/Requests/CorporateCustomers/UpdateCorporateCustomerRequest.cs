using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.CorporateCustomers;

/// <summary>
///     Request model for updating a corporate customer.
/// </summary>
public sealed class UpdateCorporateCustomerRequest
{
    /// <summary>
    ///     Gets or sets the business registration number (RC Number).
    /// </summary>
    [MaxLength(15, ErrorMessage = "RC Number cannot exceed 15 characters")]
    [JsonPropertyName("rcNumber")]
    public string? RcNumber { get; set; }

    /// <summary>
    ///     Gets or sets the Tax Identification Number (TIN).
    /// </summary>
    [MaxLength(15, ErrorMessage = "TIN cannot exceed 15 characters")]
    [JsonPropertyName("tin")]
    public string? Tin { get; set; }

    /// <summary>
    ///     Gets or sets the full business name.
    /// </summary>
    [MaxLength(150, ErrorMessage = "Full business name cannot exceed 150 characters")]
    [JsonPropertyName("fullBusinessName")]
    public string? FullBusinessName { get; set; }

    /// <summary>
    ///     Gets or sets the business address.
    /// </summary>
    [MaxLength(250, ErrorMessage = "Business address cannot exceed 250 characters")]
    [JsonPropertyName("businessAddress")]
    public string? BusinessAddress { get; set; }

    /// <summary>
    ///     Gets or sets the country ID.
    /// </summary>
    [JsonPropertyName("countryId")]
    public Guid CountryId { get; set; }

    /// <summary>
    ///     Gets or sets the business city.
    /// </summary>
    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>
    ///     Gets or sets the business email address.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    ///     Gets or sets the wallet preferred name.
    /// </summary>
    [MaxLength(100, ErrorMessage = "Wallet preferred name cannot exceed 100 characters")]
    [JsonPropertyName("walletPreferredName")]
    public string? WalletPreferredName { get; set; }
}