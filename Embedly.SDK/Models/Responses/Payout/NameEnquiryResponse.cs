using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
///     Response model for bank account name enquiry.
/// </summary>
public sealed class NameEnquiryResponse
{
    /// <summary>
    ///     Gets or sets the unique session identifier for the transaction.
    /// </summary>
    [JsonPropertyName("sessionID")]
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the destination institution code associated with the transaction.
    /// </summary>
    [JsonPropertyName("destinationInstitutionCode")]
    public string DestinationInstitutionCode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the channel code used for the transaction.
    /// </summary>
    [JsonPropertyName("channelCode")]
    public int ChannelCode { get; set; }

    /// <summary>
    ///     Gets or sets the customer's account number.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's account name.
    /// </summary>
    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's KYC (Know Your Customer) level.
    /// </summary>
    [JsonPropertyName("kycLevel")]
    public string KycLevel { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the response code indicating the outcome of the verification.
    /// </summary>
    [JsonPropertyName("responseCode")]
    public string ResponseCode { get; set; } = string.Empty;
}