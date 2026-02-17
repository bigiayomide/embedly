using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
///     Response from creating a new wallet.
/// </summary>
public sealed class CreateWalletResponse
{
    /// <summary>
    ///     Gets or sets the response message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    ///     Gets or sets the created wallet ID.
    /// </summary>
    [JsonPropertyName("walletId")]
    public string? WalletId { get; set; }

    /// <summary>
    ///     Gets or sets the virtual account number.
    /// </summary>
    [JsonPropertyName("virtualAccount")]
    public string? VirtualAccount { get; set; }

    /// <summary>
    ///     Gets or sets the mobile number associated with the wallet.
    /// </summary>
    [JsonPropertyName("mobNum")]
    public string? MobNum { get; set; }
}
