using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
///     Response model for bank account name enquiry.
/// </summary>
public sealed class NameEnquiryResponse
{
    /// <summary>
    ///     Gets or sets the overall status of the verification response.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the detailed bank account verification data.
    /// </summary>
    [JsonPropertyName("data")]
    public BankAccountVerificationData Data { get; set; } = new();

    /// <summary>
    ///     Gets or sets the response code returned from the verification service.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    ///     Gets or sets the message returned from the verification service.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

/// <summary>
/// Represents the detailed data for the bank account verification response.
/// </summary>
public class BankAccountVerificationData
{
    /// <summary>
    ///     Gets or sets the destination bank code associated with the account.
    /// </summary>
    [JsonPropertyName("destinationBankCode")]
    public string DestinationBankCode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the verified bank account number.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the verified name associated with the bank account.
    /// </summary>
    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;
}