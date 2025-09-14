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
    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the bank name.
    /// </summary>
    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets whether the bank is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    ///     Gets or sets the bank type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}