using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
///     Customer verification properties and KYC status information.
/// </summary>
public sealed class CustomerVerificationProperties
{
    /// <summary>
    ///     Gets or sets the unique identifier of the customer.
    /// </summary>
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a value indicating whether the customer has a registered BVN (Bank Verification Number).
    /// </summary>
    [JsonPropertyName("hasBvn")]
    public bool HasBvn { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the customer has a registered NIN (National Identification Number).
    /// </summary>
    [JsonPropertyName("hasNin")]
    public bool HasNin { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the customer's address has been verified.
    /// </summary>
    [JsonPropertyName("hasAddressVerification")]
    public bool HasAddressVerification { get; set; }

    /// <summary>
    ///     Gets or sets the customer's verification tier level.
    /// </summary>
    [JsonPropertyName("customerTierId")]
    public int CustomerTierId { get; set; }

    /// <summary>
    ///     Gets or sets the customer's BVN value, if available.
    /// </summary>
    [JsonPropertyName("bvnValue")]
    public string BvnValue { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's NIN value, if available.
    /// </summary>
    [JsonPropertyName("ninValue")]
    public string NinValue { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the verification status.
    /// </summary>
    [JsonPropertyName("verificationStatus")]
    public CustomerVerificationStatus VerificationStatus { get; set; }

    /// <summary>
    ///     Gets or sets the maximum transaction limits for this KYC level.
    /// </summary>
    [JsonPropertyName("transactionLimits")]
    public TransactionLimits? TransactionLimits { get; set; }
}

/// <summary>
///     Transaction limits for a KYC level.
/// </summary>
public sealed class TransactionLimits
{
    /// <summary>
    ///     Gets or sets the daily transaction limit.
    /// </summary>
    [JsonPropertyName("dailyLimit")]
    public long DailyLimit { get; set; }

    /// <summary>
    ///     Gets or sets the monthly transaction limit.
    /// </summary>
    [JsonPropertyName("monthlyLimit")]
    public long MonthlyLimit { get; set; }

    /// <summary>
    ///     Gets or sets the single transaction limit.
    /// </summary>
    [JsonPropertyName("singleTransactionLimit")]
    public long SingleTransactionLimit { get; set; }

    /// <summary>
    ///     Gets or sets the currency for the limits.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";
}