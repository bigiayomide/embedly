using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
///     Represents the response returned from the KYC address verification request.
/// </summary>
public class AddressKycUpgradeResponse
{
    /// <summary>
    ///     Gets or sets the status of the verification response.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the message describing the result of the verification.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the timestamp of when the verification was processed.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    ///     Gets or sets the verification data returned from the process.
    /// </summary>
    [JsonPropertyName("data")]
    public AddressVerificationData? Data { get; set; }
}

/// <summary>
///     Represents the detailed data of the address verification.
/// </summary>
public class AddressVerificationData
{
    /// <summary>
    ///     Gets or sets a value indicating whether the address was successfully verified.
    /// </summary>
    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    /// <summary>
    ///     Gets or sets the verified house address.
    /// </summary>
    [JsonPropertyName("houseAddress")]
    public string HouseAddress { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the house owner.
    /// </summary>
    [JsonPropertyName("houseOwner")]
    public string HouseOwner { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the confidence level of the verification result.
    /// </summary>
    [JsonPropertyName("confidenceLevel")]
    public int ConfidenceLevel { get; set; }

    /// <summary>
    ///     Gets or sets the electricity distribution company code (DisCo code).
    /// </summary>
    [JsonPropertyName("discoCode")]
    public string DiscoCode { get; set; } = string.Empty;
}
