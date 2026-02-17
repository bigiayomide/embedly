using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for simulating wallet inflow (staging environment only).
/// </summary>
public sealed record SimulateInflowRequest
{
    /// <summary>
    ///     Gets or sets the beneficiary account name.
    /// </summary>
    [Required(ErrorMessage = "Beneficiary account name is required")]
    [JsonPropertyName("beneficiaryAccountName")]
    public string BeneficiaryAccountName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the beneficiary account number.
    /// </summary>
    [Required(ErrorMessage = "Beneficiary account number is required")]
    [JsonPropertyName("beneficiaryAccountNumber")]
    public string BeneficiaryAccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the amount to credit.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [JsonPropertyName("amount")]
    public string Amount { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the narration/remarks for the transaction.
    /// </summary>
    [Required(ErrorMessage = "Narration is required")]
    [JsonPropertyName("narration")]
    public string Narration { get; init; } = string.Empty;
}
