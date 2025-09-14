using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for searching wallets by email.
/// </summary>
public sealed record SearchWalletByEmailRequest
{
    /// <summary>
    ///     Gets or sets the email address to search for.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;
}