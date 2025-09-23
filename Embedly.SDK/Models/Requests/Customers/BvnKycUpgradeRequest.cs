using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
///     Request model for upgrading customer KYC using BVN (Bank Verification Number).
/// </summary>
public sealed class BvnKycUpgradeRequest
{
    /// <summary>
    ///     Gets or sets the customer identifier.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the Bank Verification Number (BVN).
    /// </summary>
    [Required(ErrorMessage = "BVN is required")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "BVN must be 11 digits")]
    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;
}