using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Cards;

/// <summary>
/// Request model for updating Afrigo card information based on AdminUpdateCardInfoRequestDto schema.
/// </summary>
public sealed record UpdateAfrigoCardRequest
{
    /// <summary>
    /// Gets or sets the account number (required).
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [MinLength(1, ErrorMessage = "Account number cannot be empty")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the card number.
    /// </summary>
    [JsonPropertyName("cardNumber")]
    public string? CardNumber { get; init; }
    
    /// <summary>
    /// Gets or sets the old mobile number.
    /// </summary>
    [JsonPropertyName("oldMobile")]
    public string? OldMobile { get; init; }
    
    /// <summary>
    /// Gets or sets the new mobile number.
    /// </summary>
    [JsonPropertyName("newMobile")]
    public string? NewMobile { get; init; }
}