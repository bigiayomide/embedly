using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Payout;

/// <summary>
///     Represents a bank in the Embedly system.
/// </summary>
public sealed class Bank
{
    /// <summary>
    ///     Gets or sets the bank code.
    /// </summary>
    [JsonPropertyName("bankcode")]
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the bank name.
    /// </summary>
    [JsonPropertyName("bankname")]
    public string BankName { get; set; } = string.Empty;
}