using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
/// Request model for searching wallets by mobile number.
/// </summary>
public sealed record SearchWalletByMobileRequest
{
    /// <summary>
    /// Gets or sets the mobile number to search for.
    /// </summary>
    [Required(ErrorMessage = "Mobile number is required")]
    [JsonPropertyName("mobileNumber")]
    public string MobileNumber { get; init; } = string.Empty;
}