using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
/// Response model for payout profile creation.
/// </summary>
public sealed class PayoutProfileResponse
{
    /// <summary>
    /// Gets or sets the profile ID.
    /// </summary>
    [JsonPropertyName("profileId")]
    public Guid ProfileId { get; set; }
    
    /// <summary>
    /// Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// Gets or sets the wallet ID.
    /// </summary>
    [JsonPropertyName("walletId")]
    public Guid WalletId { get; set; }
    
    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}