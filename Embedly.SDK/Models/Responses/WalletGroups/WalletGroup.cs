using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.WalletGroups;

/// <summary>
///     Represents a wallet group based on WalletGroupDto schema.
/// </summary>
public sealed class WalletGroup
{
    /// <summary>
    ///     Gets or sets the unique wallet group identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the wallet group name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     Gets or sets the wallet group features.
    /// </summary>
    [JsonPropertyName("walletGroupFeatureDto")]
    public List<WalletGroupFeature>? WalletGroupFeatures { get; set; }
}