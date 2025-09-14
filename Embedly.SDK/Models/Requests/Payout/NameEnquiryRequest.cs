using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Payout;

/// <summary>
///     Request model for bank account name enquiry.
/// </summary>
public sealed class NameEnquiryRequest
{
    /// <summary>
    ///     Gets or sets the bank account number.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the bank code.
    /// </summary>
    [Required(ErrorMessage = "Bank code is required")]
    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;
}