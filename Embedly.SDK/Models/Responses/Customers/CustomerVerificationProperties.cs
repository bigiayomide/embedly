using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
/// Customer verification properties and KYC status information.
/// </summary>
public sealed class CustomerVerificationProperties
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the current KYC level.
    /// </summary>
    [JsonPropertyName("kycLevel")]
    public string KycLevel { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the verification status.
    /// </summary>
    [JsonPropertyName("verificationStatus")]
    public CustomerVerificationStatus VerificationStatus { get; set; }
    
    /// <summary>
    /// Gets or sets whether identity verification is completed.
    /// </summary>
    [JsonPropertyName("identityVerified")]
    public bool IdentityVerified { get; set; }
    
    /// <summary>
    /// Gets or sets whether address verification is completed.
    /// </summary>
    [JsonPropertyName("addressVerified")]
    public bool AddressVerified { get; set; }
    
    /// <summary>
    /// Gets or sets whether phone number is verified.
    /// </summary>
    [JsonPropertyName("phoneVerified")]
    public bool PhoneVerified { get; set; }
    
    /// <summary>
    /// Gets or sets whether email is verified.
    /// </summary>
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }
    
    /// <summary>
    /// Gets or sets the verification methods completed.
    /// </summary>
    [JsonPropertyName("completedVerifications")]
    public List<string> CompletedVerifications { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the pending verification requirements.
    /// </summary>
    [JsonPropertyName("pendingVerifications")]
    public List<string> PendingVerifications { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the maximum transaction limits for this KYC level.
    /// </summary>
    [JsonPropertyName("transactionLimits")]
    public TransactionLimits? TransactionLimits { get; set; }
    
    /// <summary>
    /// Gets or sets the date when verification was last updated.
    /// </summary>
    [JsonPropertyName("lastVerificationUpdate")]
    public DateTime? LastVerificationUpdate { get; set; }
}

/// <summary>
/// Transaction limits for a KYC level.
/// </summary>
public sealed class TransactionLimits
{
    /// <summary>
    /// Gets or sets the daily transaction limit.
    /// </summary>
    [JsonPropertyName("dailyLimit")]
    public long DailyLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the monthly transaction limit.
    /// </summary>
    [JsonPropertyName("monthlyLimit")]
    public long MonthlyLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the single transaction limit.
    /// </summary>
    [JsonPropertyName("singleTransactionLimit")]
    public long SingleTransactionLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the currency for the limits.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";
}