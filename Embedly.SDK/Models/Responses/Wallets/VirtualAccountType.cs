using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
/// Virtual account type information.
/// </summary>
public sealed class VirtualAccountType
{
    /// <summary>
    /// Gets or sets the virtual account type ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the virtual account type name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the virtual account type description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets the bank code for this virtual account type.
    /// </summary>
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }
    
    /// <summary>
    /// Gets or sets the bank name for this virtual account type.
    /// </summary>
    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }
    
    /// <summary>
    /// Gets or sets whether this virtual account type is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}