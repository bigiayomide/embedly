using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
///     Root object for the BVN KYC response payload.
/// </summary>
public class BvnKycUpgradeResponse
{
    /// <summary>
    /// Indicates whether KYC process was completed.
    /// </summary>
    [JsonPropertyName("kycCompleted")]
    public bool KycCompleted { get; set; }

    /// <summary>
    /// The detailed response object containing identity and verification data.
    /// </summary>
    [JsonPropertyName("response")]
    public KycResultResponse? Response { get; set; }

    /// <summary>
    /// Indicates whether the overall operation was successful.
    /// </summary>
    [JsonPropertyName("successful")]
    public bool Successful { get; set; }
}
