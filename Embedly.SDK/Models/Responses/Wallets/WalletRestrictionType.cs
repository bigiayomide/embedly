using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
/// Wallet restriction type information.
/// </summary>
public sealed class WalletRestrictionType
{
    /// <summary>
    /// Gets or sets the restriction type ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the restriction type name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the restriction type description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets whether this restriction type is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}