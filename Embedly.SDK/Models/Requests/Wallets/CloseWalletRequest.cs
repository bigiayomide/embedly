using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for closing a wallet.
/// </summary>
public sealed record CloseWalletRequest
{
    /// <summary>
    ///     Gets or sets the wallet account number to close.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the reason for closing the wallet.
    /// </summary>
    [Required(ErrorMessage = "Closure reason is required")]
    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;

    /// <summary>
    ///     Specifies whether to close or delete the account. "true: Delete the account. false: Close the account"
    /// </summary>
    [Required(ErrorMessage = "Close or delete option is required")]
    [JsonPropertyName("closeOrDelete")]
    public bool CloseOrDelete { get; set; }

    /// <summary>
    ///     Specifies whether the operation is for a customer or an account. true: Customer - false: Account
    /// </summary>
    [Required(ErrorMessage = "Customer or Account option is required")]
    [JsonPropertyName("customerOrAccount")]
    public bool CustomerOrAccount { get; set; }

    /// <summary>
    ///     The ID of the teller processing the closure.
    /// </summary>
    [JsonPropertyName("tellerId")]
    public string TellerId { get; set; } = string.Empty;
}