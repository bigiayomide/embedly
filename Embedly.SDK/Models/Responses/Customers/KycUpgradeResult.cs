using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
///     Result of a KYC upgrade operation.
/// </summary>
public sealed class KycUpgradeResult
{
    /// <summary>
    ///     Gets or sets whether the KYC upgrade was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    ///     Gets or sets the result message.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer identifier.
    /// </summary>
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the new KYC level after upgrade.
    /// </summary>
    [JsonPropertyName("newKycLevel")]
    public string? NewKycLevel { get; set; }

    /// <summary>
    ///     Gets or sets the verification status.
    /// </summary>
    [JsonPropertyName("verificationStatus")]
    public CustomerVerificationStatus VerificationStatus { get; set; }

    /// <summary>
    ///     Gets or sets the verification reference ID.
    /// </summary>
    [JsonPropertyName("verificationReference")]
    public string? VerificationReference { get; set; }

    /// <summary>
    ///     Gets or sets the date when the upgrade was processed.
    /// </summary>
    [JsonPropertyName("processedAt")]
    public DateTime ProcessedAt { get; set; }
}

/// <summary>
///     Result of an address verification operation.
/// </summary>
public sealed class AddressVerificationResult
{
    /// <summary>
    ///     Gets or sets whether the address verification was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    ///     Gets or sets the result message.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer identifier.
    /// </summary>
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the verification status.
    /// </summary>
    [JsonPropertyName("verificationStatus")]
    public string VerificationStatus { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the verification reference ID.
    /// </summary>
    [JsonPropertyName("verificationReference")]
    public string? VerificationReference { get; set; }

    /// <summary>
    ///     Gets or sets the verified address information.
    /// </summary>
    [JsonPropertyName("verifiedAddress")]
    public Address? VerifiedAddress { get; set; }

    /// <summary>
    ///     Gets or sets the date when the verification was processed.
    /// </summary>
    [JsonPropertyName("processedAt")]
    public DateTime ProcessedAt { get; set; }
}