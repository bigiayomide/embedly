using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Responses.Wallets;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
/// Request model for creating a corporate customer wallet based on CreateWalletForCorporateCustomerDto.
/// </summary>
public sealed record CreateCorporateWalletRequest
{
    /// <summary>
    /// Gets or sets the currency ID.
    /// </summary>
    [Required(ErrorMessage = "Currency ID is required")]
    [JsonPropertyName("currencyId")]
    public Guid CurrencyId { get; init; }
    
    /// <summary>
    /// Gets or sets the virtual account information.
    /// </summary>
    [JsonPropertyName("virtualAccount")]
    public WalletVirtualAccount? VirtualAccount { get; init; }
    
    /// <summary>
    /// Gets or sets the wallet name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}