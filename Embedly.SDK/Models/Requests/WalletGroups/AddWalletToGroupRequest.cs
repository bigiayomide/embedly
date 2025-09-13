using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.WalletGroups;

/// <summary>
/// Request model for adding a wallet to a wallet group.
/// </summary>
public sealed class AddWalletToGroupRequest
{
    /// <summary>
    /// Gets or sets the wallet group ID.
    /// </summary>
    [Required(ErrorMessage = "Wallet group ID is required")]
    [JsonPropertyName("walletGroupId")]
    public string WalletGroupId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the wallet ID to add to the group.
    /// </summary>
    [Required(ErrorMessage = "Wallet ID is required")]
    [JsonPropertyName("walletId")]
    public string WalletId { get; set; } = string.Empty;
}