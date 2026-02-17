using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Cards;

/// <summary>
///     Request model for resetting an Afrigo card PIN.
/// </summary>
public sealed record ResetCardPinRequest
{
    /// <summary>
    ///     Gets or sets the account number.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the card number.
    /// </summary>
    [Required(ErrorMessage = "Card number is required")]
    [JsonPropertyName("cardNumber")]
    public string CardNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the new PIN (will be encrypted automatically).
    /// </summary>
    [Required(ErrorMessage = "PIN is required")]
    [JsonPropertyName("pin")]
    public string Pin { get; init; } = string.Empty;
}
