using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.WalletGroups;

/// <summary>
/// Request model for creating a new wallet group.
/// </summary>
public sealed class CreateWalletGroupRequest
{
    /// <summary>
    /// Gets or sets the wallet group name.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the wallet group description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets the customer ID who will own the wallet group.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets additional metadata for the wallet group.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object?>? Metadata { get; set; }
}