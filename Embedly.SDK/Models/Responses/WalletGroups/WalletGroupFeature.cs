using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.WalletGroups;

/// <summary>
///     Represents a wallet group feature based on WalletGroupFeatureDto schema.
/// </summary>
public sealed class WalletGroupFeature
{
    /// <summary>
    ///     Gets or sets the feature name.
    /// </summary>
    [JsonPropertyName("featureName")]
    public string? FeatureName { get; set; }

    /// <summary>
    ///     Gets or sets the feature property name.
    /// </summary>
    [JsonPropertyName("featurePropertyName")]
    public string? FeaturePropertyName { get; set; }

    /// <summary>
    ///     Gets or sets the feature property value.
    /// </summary>
    [JsonPropertyName("featurePropertyValue")]
    public string? FeaturePropertyValue { get; set; }
}