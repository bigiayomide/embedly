using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Cards;

/// <summary>
///     Request model for issuing an Afrigo card based on AdminCardIssueRequestDto schema.
/// </summary>
public sealed record IssueAfrigoCardRequest
{
    /// <summary>
    ///     Gets or sets the account number (required).
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [MinLength(1, ErrorMessage = "Account number cannot be empty")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the pickup method for the card.
    /// </summary>
    [JsonPropertyName("pickupMethod")]
    public string? PickupMethod { get; init; }

    /// <summary>
    ///     Gets or sets the card type.
    /// </summary>
    [JsonPropertyName("cardType")]
    public string? CardType { get; init; }

    /// <summary>
    ///     Gets or sets the ID number for verification.
    /// </summary>
    [JsonPropertyName("idNo")]
    public string? IdNo { get; init; }

    /// <summary>
    ///     Gets or sets the ID type for verification.
    /// </summary>
    [JsonPropertyName("idType")]
    public string? IdType { get; init; }

    /// <summary>
    ///     Gets or sets the customer's email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    ///     Gets or sets the customer's address.
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; init; }

    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid? OrganizationId { get; init; }
}