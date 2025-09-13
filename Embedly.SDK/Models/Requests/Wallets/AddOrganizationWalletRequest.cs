using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Responses.Wallets;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
/// Request model for adding a wallet to an organization.
/// </summary>
public sealed record AddOrganizationWalletRequest
{
    /// <summary>
    /// Gets or sets the organization ID.
    /// </summary>
    [Required(ErrorMessage = "Organization ID is required")]
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }
    
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public Guid CustomerId { get; init; }
    
    /// <summary>
    /// Gets or sets the currency ID.
    /// </summary>
    [Required(ErrorMessage = "Currency ID is required")]
    [JsonPropertyName("currencyId")]
    public Guid CurrencyId { get; init; }
    
    /// <summary>
    /// Gets or sets the wallet name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    
    /// <summary>
    /// Gets or sets the virtual account information.
    /// </summary>
    [JsonPropertyName("virtualAccount")]
    public WalletVirtualAccount? VirtualAccount { get; init; }
}