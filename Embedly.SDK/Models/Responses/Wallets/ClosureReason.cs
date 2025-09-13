using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
/// Represents a wallet closure reason.
/// </summary>
public sealed record ClosureReason
{
    /// <summary>
    /// Gets or sets the closure reason ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the closure reason name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the closure reason description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets or sets whether this closure reason is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }
}