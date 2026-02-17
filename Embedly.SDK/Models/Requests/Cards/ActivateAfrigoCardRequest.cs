using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Cards;

/// <summary>
///     Request model for activating an Afrigo card based on AdminCardActivationRequestDto schema.
/// </summary>
public sealed record ActivateAfrigoCardRequest
{
    /// <summary>
    ///     Gets or sets the account number (required).
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [MinLength(1, ErrorMessage = "Account number cannot be empty")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the card number (masked format: first 6 digits + asterisks + last 4 digits).
    /// </summary>
    [Required(ErrorMessage = "Card number is required")]
    [JsonPropertyName("cardNumber")]
    public string CardNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the PIN for the card (will be encrypted automatically using RSA).
    /// </summary>
    [Required(ErrorMessage = "PIN is required")]
    [JsonPropertyName("pin")]
    public string Pin { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid? OrganizationId { get; init; }
}