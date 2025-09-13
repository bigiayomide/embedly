using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
/// Wallet type information.
/// </summary>
public sealed class WalletType
{
    /// <summary>
    /// Gets or sets the wallet type ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the wallet type name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the wallet type description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets whether this wallet type is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}