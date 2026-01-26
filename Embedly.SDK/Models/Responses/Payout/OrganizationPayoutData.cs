using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
///     Represents organization payout data.
/// </summary>
public sealed class OrganizationPayoutData
{
    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the organization name.
    /// </summary>
    [JsonPropertyName("organizationName")]
    public string? OrganizationName { get; set; }

    /// <summary>
    ///     Gets or sets the total payout amount.
    /// </summary>
    [JsonPropertyName("totalPayoutAmount")]
    public decimal TotalPayoutAmount { get; set; }

    /// <summary>
    ///     Gets or sets the payout count.
    /// </summary>
    [JsonPropertyName("payoutCount")]
    public int PayoutCount { get; set; }

    /// <summary>
    ///     Gets or sets the last payout date.
    /// </summary>
    [JsonPropertyName("lastPayoutDate")]
    public DateTime? LastPayoutDate { get; set; }
}