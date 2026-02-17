using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Responses.Wallets;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for creating a new wallet based on WalletDto schema.
/// </summary>
public sealed class CreateWalletRequest
{
    /// <summary>
    ///     Gets or sets the customer ID who will own the wallet.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public string? CustomerId { get; set; }

    /// <summary>
    ///     Gets or sets the currency ID for the wallet.
    /// </summary>
    [Required(ErrorMessage = "Currency ID is required")]
    [JsonPropertyName("currencyId")]
    public string? CurrencyId { get; set; }

    /// <summary>
    ///     Gets or sets the wallet restriction ID.
    /// </summary>
    [JsonPropertyName("walletRestrictionId")]
    public string? WalletRestrictionId { get; set; }

    /// <summary>
    ///     Gets or sets the wallet classification ID.
    /// </summary>
    [JsonPropertyName("walletClassificationId")]
    public string? WalletClassificationId { get; set; }

    /// <summary>
    ///     Gets or sets the customer type ID.
    /// </summary>
    [JsonPropertyName("customerTypeId")]
    public string? CustomerTypeId { get; set; }

    /// <summary>
    ///     Gets or sets the wallet group ID.
    /// </summary>
    [JsonPropertyName("walletGroupId")]
    public string? WalletGroupId { get; set; }

    /// <summary>
    ///     Gets or sets whether the wallet is internal.
    /// </summary>
    [JsonPropertyName("isInternal")]
    public bool IsInternal { get; set; }

    /// <summary>
    ///     Gets or sets whether the wallet is the default wallet.
    /// </summary>
    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; set; }

    /// <summary>
    ///     Gets or sets the wallet name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     Gets or sets the overdraft limit.
    /// </summary>
    [JsonPropertyName("overdraft")]
    public decimal? Overdraft { get; set; }

    /// <summary>
    ///     Gets or sets the virtual account information.
    /// </summary>
    [JsonPropertyName("virtualAccount")]
    public WalletVirtualAccount? VirtualAccount { get; set; }

    /// <summary>
    ///     Gets or sets the mobile number associated with the wallet.
    /// </summary>
    [JsonPropertyName("mobNum")]
    public string? MobNum { get; set; }
}