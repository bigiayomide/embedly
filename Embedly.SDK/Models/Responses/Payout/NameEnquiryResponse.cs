using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
///     Response model for bank account name enquiry.
/// </summary>
public sealed class NameEnquiryResponse
{
    /// <summary>
    ///     Gets or sets the destination bank code.
    /// </summary>
    [JsonPropertyName("destinationBankCode")]
    public string DestinationBankCode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the account number.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the account name.
    /// </summary>
    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;
}
