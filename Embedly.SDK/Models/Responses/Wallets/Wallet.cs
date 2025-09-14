using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
///     Represents a wallet in the Embedly system based on WalletDto schema.
/// </summary>
public sealed class Wallet
{
    /// <summary>
    ///     Gets or sets the unique wallet identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the wallet group ID.
    /// </summary>
    [JsonPropertyName("walletGroupId")]
    public Guid? WalletGroupId { get; set; }

    /// <summary>
    ///     Gets or sets the customer ID who owns the wallet.
    /// </summary>
    [JsonPropertyName("customerId")]
    public Guid? CustomerId { get; set; }

    /// <summary>
    ///     Gets or sets the available balance.
    /// </summary>
    [JsonPropertyName("availableBalance")]
    public double AvailableBalance { get; set; }

    /// <summary>
    ///     Gets or sets the ledger balance.
    /// </summary>
    [JsonPropertyName("ledgerBalance")]
    public double LedgerBalance { get; set; }

    /// <summary>
    ///     Gets or sets the wallet restriction ID.
    /// </summary>
    [JsonPropertyName("walletRestrictionId")]
    public Guid? WalletRestrictionId { get; set; }

    /// <summary>
    ///     Gets or sets the wallet classification ID.
    /// </summary>
    [JsonPropertyName("walletClassificationId")]
    public Guid WalletClassificationId { get; set; }

    /// <summary>
    ///     Gets or sets the currency ID.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public Guid CurrencyId { get; set; }

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
    public double? Overdraft { get; set; }

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

    /// <summary>
    ///     Gets or sets the customer type ID.
    /// </summary>
    [JsonPropertyName("customerTypeId")]
    public Guid CustomerTypeId { get; set; }
}

/// <summary>
///     Wallet virtual account information.
/// </summary>
public sealed class WalletVirtualAccount
{
    /// <summary>
    ///     Gets or sets the account number.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the bank code.
    /// </summary>
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }

    /// <summary>
    ///     Gets or sets the bank name.
    /// </summary>
    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }
}

/// <summary>
///     Wallet status enumeration.
/// </summary>
public enum WalletStatus
{
    /// <summary>
    ///     Wallet is active and can be used for transactions.
    /// </summary>
    [JsonPropertyName("active")] Active,

    /// <summary>
    ///     Wallet is inactive.
    /// </summary>
    [JsonPropertyName("inactive")] Inactive,

    /// <summary>
    ///     Wallet is suspended.
    /// </summary>
    [JsonPropertyName("suspended")] Suspended,

    /// <summary>
    ///     Wallet is closed.
    /// </summary>
    [JsonPropertyName("closed")] Closed
}