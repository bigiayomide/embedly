using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.WalletGroups;

/// <summary>
///     Request model for adding a feature to a wallet group based on AddWalletFeatureDto.
/// </summary>
public sealed record AddWalletGroupFeatureRequest
{
    /// <summary>
    ///     Gets or sets the wallet group ID.
    /// </summary>
    [Required(ErrorMessage = "Group ID is required")]
    [JsonPropertyName("groupId")]
    public Guid GroupId { get; init; }

    /// <summary>
    ///     Gets or sets the feature ID to add.
    /// </summary>
    [Required(ErrorMessage = "Feature ID is required")]
    [JsonPropertyName("featureId")]
    public Guid FeatureId { get; init; }

    /// <summary>
    ///     Gets or sets parameter 1.
    /// </summary>
    [JsonPropertyName("param1")]
    public double? Param1 { get; init; }

    /// <summary>
    ///     Gets or sets parameter 2.
    /// </summary>
    [JsonPropertyName("param2")]
    public int? Param2 { get; init; }
}