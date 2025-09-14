using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
///     Represents a wallet restriction type.
/// </summary>
public sealed record RestrictionType
{
    /// <summary>
    ///     Gets or sets the restriction type ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the restriction type name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the restriction type description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    ///     Gets or sets whether this restriction type is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }
}